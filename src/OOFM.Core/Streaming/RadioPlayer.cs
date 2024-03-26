﻿using OOFM.Core.Api;
using OOFM.Core.Models;
using OOFM.Core.Streaming.M3U;

namespace OOFM.Core.Streaming;

public class RadioPlayer : IRadioPlayer
{
    private readonly M3UWebStream _m3uStream;

    public event Action<Station>? PlaybackStarted;
    public event Action<Station>? PlaybackStopped;

    private CancellationTokenSource? _cts;
    private Task? _streamTask;

    public Station? CurrentStation { get; private set; }

    public RadioPlayer(IHttpClientProvider httpClientProvider)
    {
        _m3uStream = new M3UWebStream(httpClientProvider.GetHttpClient(), 262144 /*256kb*/);
    }

    public void Play(Station station)
    {
        Stop();

        if (station is not null)
        {
            CurrentStation = station;
            PlaybackStarted?.Invoke(CurrentStation);

            if (!string.IsNullOrEmpty(CurrentStation.StreamUrl))
            {
                _m3uStream.BeginStreaming(CurrentStation.StreamUrl);

                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                _streamTask = Task.Run(async () =>
                {
                    try
                    {
                        await StreamLoop(CurrentStation.StreamUrl, _cts.Token);
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

    private async Task StreamLoop(string streamUrl, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(100);
        }
    }
}
