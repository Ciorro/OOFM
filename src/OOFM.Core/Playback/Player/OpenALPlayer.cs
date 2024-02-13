using OpenTK.Audio.OpenAL;

namespace OOFM.Core.Playback.Player;

public class OpenALPlayer : IPlayer, IDisposable
{
    private const int NumBuffers = 16;
    private const int BufferSize = 2048;

    private readonly Stream _stream;

    private readonly ALDevice _device;
    private readonly ALContext _context;

    private readonly int[] _buffers;
    private readonly int _source;

    private CancellationTokenSource? _cts;
    private Task? _playbackTask;

    public OpenALPlayer(Stream stream)
    {
        _stream = stream;

        //Initialize OpenAL
        _device = ALC.OpenDevice(null);
        _context = ALC.CreateContext(_device, new ALContextAttributes());
        ALC.MakeContextCurrent(_context);

        //Create buffers
        _buffers = new int[NumBuffers];
        AL.GenBuffers(_buffers);

        //Create source
        AL.GenSource(out _source);
        AL.Source(_source, ALSourcef.Gain, 1);
    }

    public bool IsPlaying
    {
        get => (ALSourceState)AL.GetSource(_source, ALGetSourcei.SourceState) == ALSourceState.Playing;
    }

    public float Volume
    {
        get
        {
            return AL.GetSource(_source, ALSourcef.Gain);
        }
        set
        {
            AL.Source(_source, ALSourcef.Gain, value);
        }
    }

    public void Play()
    {
        _cts = new CancellationTokenSource();

        _playbackTask = Task.Run(async () =>
        {
            try
            {
                await PlaybackLoop(_cts.Token);
            }
            catch
            {
                AL.SourceStop(_source);
                AL.SourceUnqueueBuffers(_source, _buffers);
            }
        });
    }

    public void Stop()
    {
        _cts?.Cancel();
        _playbackTask?.Wait();
    }

    public void Dispose()
    {
        Stop();

        //OpenAL cleanup
        AL.DeleteSource(_source);
        AL.DeleteBuffers(_buffers);
        ALC.CloseDevice(_device);
        ALC.DestroyContext(_context);
    }

    private async Task PlaybackLoop(CancellationToken cancellationToken)
    {
        byte[] dataBuffer = new byte[BufferSize];

        //Initialize buffers
        for (int i = 0; i < NumBuffers; i++)
        {
            await _stream.ReadAsync(dataBuffer, cancellationToken);

            AL.BufferData(_buffers[i], ALFormat.Stereo16, dataBuffer, 48000);
            AL.SourceQueueBuffer(_source, _buffers[i]);
        }

        AL.SourcePlay(_source);

        //Update buffers
        while (!cancellationToken.IsCancellationRequested)
        {
            while (AL.GetSource(_source, ALGetSourcei.BuffersProcessed) > 0)
            {
                await _stream.ReadAsync(dataBuffer, cancellationToken);

                int alBuffer = AL.SourceUnqueueBuffer(_source);

                AL.BufferData(alBuffer, ALFormat.Stereo16, dataBuffer, 48000);
                AL.SourceQueueBuffer(_source, alBuffer);
            }

            await Task.Delay(10);
        }

        AL.SourceStop(_source);
        AL.SourceUnqueueBuffers(_source, _buffers);
    }
}
