using System.Diagnostics;

namespace OOFM.Core.Streaming.Decoding;

public class FFmpegDecoder : IDecoder, IDisposable
{
    private readonly Process _ffmpeg;

    private readonly SoundBuffer _inBuffer;
    private readonly SoundBuffer _outBuffer;

    private readonly CancellationTokenSource _cts;
    private readonly Task _inputReaderTask;
    private readonly Task _outputReaderTask;

    public FFmpegDecoder(string ffmpegPath)
    {
        _ffmpeg = Process.Start(new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = "-i - -f s16le -",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        })!;

        _inBuffer = new SoundBuffer(1048576);
        _outBuffer = new SoundBuffer(1048576);

        _cts = new CancellationTokenSource();

        _inputReaderTask = Task.Run(async () =>
        {
            var cancellationToken = _cts.Token;
            byte[] buffer = new byte[4096];

            while (!cancellationToken.IsCancellationRequested)
            {
                int read = _inBuffer.Read(buffer, 0, buffer.Length);

                if (read > 0)
                {
                    _ffmpeg.StandardInput.BaseStream.Write(buffer, 0, read);
                    _ffmpeg.StandardInput.Flush();
                }

                await Task.Delay(10);
            }
        });

        _outputReaderTask = Task.Run(async () =>
        {
            var cancellationToken = _cts.Token;
            byte[] buffer = new byte[4096];

            while (!cancellationToken.IsCancellationRequested)
            {
                int read = _ffmpeg.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

                if (read > 0)
                {
                    _outBuffer.Write(buffer, 0, read);
                }

                await Task.Delay(10);
            }
        });
    }

    public void ProcessInput(byte[] buffer, int offset, int count)
    {
        _inBuffer.Write(buffer, offset, count);
    }

    public int ProcessOutput(byte[] buffer, int offset, int count)
    {
        return _outBuffer.Read(buffer, offset, count);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _ffmpeg.Kill();

        Task.WaitAll(
            _inputReaderTask.WaitAsync(CancellationToken.None),
            _outputReaderTask.WaitAsync(CancellationToken.None)
        );
    }
}
