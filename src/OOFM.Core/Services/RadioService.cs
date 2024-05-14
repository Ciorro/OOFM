using OOFM.Core.Api;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Api.Models;
using OOFM.Core.Streaming;
using OOFM.Core.Streaming.Decoding;
using OOFM.Core.Streaming.M3U;
using OOFM.Core.Streaming.Playback;

namespace OOFM.Core.Services;

public class RadioService : IRadioService
{
    private readonly M3UWebStream _m3uStream;
    private readonly IUserController _userController;

    public event Action<Station>? PlaybackStarted;
    public event Action<Station>? PlaybackStopped;

    private CancellationTokenSource? _cts;
    private Task? _streamTask;

    public Station? CurrentStation { get; private set; }
    public float Volume { get; set; } = 1;
    public bool IsMuted { get; set; } = false;

    public RadioService(IHttpClientProvider httpClientProvider, IUserController userController)
    {
        _userController = userController;
        _m3uStream = new M3UWebStream(httpClientProvider.GetHttpClient(), 262144);
    }

    public void Play(Station station)
    {
        if (station.Id == CurrentStation?.Id)
        {
            return;
        }

        Stop();

        if (station is not null)
        {
            CurrentStation = station;
            PlaybackStarted?.Invoke(CurrentStation);

            if (!string.IsNullOrEmpty(CurrentStation.StreamUrl))
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                _streamTask = Task.Run(async () =>
                {
                    try
                    {
                        string urlWithToken = await _userController.AppendToken(CurrentStation.StreamUrl, _cts.Token);
                        _m3uStream.BeginStreaming(urlWithToken);

                        await StreamLoop(_cts.Token);
                    }
                    catch (Exception e)
                    {
                        await Console.Out.WriteLineAsync(e.Message);
                    }
                });
            }
        }
    }

    public void Stop()
    {
        if (CurrentStation is not null)
        {
            _m3uStream.StopStreaming();

            _cts?.Cancel();
            _streamTask?.Wait();

            PlaybackStopped?.Invoke(CurrentStation);
            CurrentStation = null;
        }
    }

    private async Task StreamLoop(CancellationToken cancellationToken)
    {
        var buffer = new byte[64000];
        var soundBuffer = new SoundBuffer(1048576 * 10);

        using (var _decoder = new FFmpegDecoder("ffmpeg"))
        using (var player = new OpenALPlayer(soundBuffer))
        {
            player.Play();

            while (!cancellationToken.IsCancellationRequested)
            {
                player.Volume = IsMuted ? 0 : Volume;

                int read;

                if ((read = _m3uStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _decoder.ProcessInput(buffer, 0, read);
                }

                if ((read = _decoder.ProcessOutput(buffer, 0, buffer.Length)) > 0)
                {
                    soundBuffer.Write(buffer, 0, read);
                }

                await Task.Delay(10);
            }
        }
    }
}
