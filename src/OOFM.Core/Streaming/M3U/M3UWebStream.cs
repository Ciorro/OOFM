namespace OOFM.Core.Streaming.M3U
{
    internal class M3UWebStream : Stream
    {
        private readonly SoundBuffer _buffer;
        private readonly HttpClient _http;

        private CancellationTokenSource? _cts;
        private Task? _streamTask;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => _buffer.Length;
        public override long Position
        {
            get => _buffer.Position;
            set => _buffer.Position = value;
        }

        public M3UWebStream(HttpClient http, int bufferSize)
        {
            _http = http;
            _buffer = new SoundBuffer(bufferSize);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _buffer.Read(buffer, offset, count);
        }

        public void BeginStreaming(string streamUrl)
        {
            StopStreaming();

            _cts = new CancellationTokenSource();
            _streamTask = Task.Run(async () =>
            {
                try
                {
                    await StreamLoop(streamUrl, _cts.Token);
                }
                catch(OperationCanceledException) { }
                catch
                {
                    //TODO: Log error
                    await Console.Out.WriteLineAsync("M3UWEBSTREAM ERROR");
                }
            });
        }

        public void StopStreaming()
        {
            _cts?.Cancel();
            _streamTask?.Wait();
        }

        private async Task StreamLoop(string streamUrl, CancellationToken cancellationToken)
        {
            int urlBaseLength = streamUrl.LastIndexOf('/') + 1;
            string baseUrl = streamUrl[..urlBaseLength];

            string m3uPlaylist = await _http.GetStringAsync(streamUrl, cancellationToken);
            string chunklistFilename = M3UParser.GetChunklistFilenameFromM3U(m3uPlaylist);

            int currentSequence = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                var chunkInfos = await GetChunks(
                    chunklistUrl: baseUrl + chunklistFilename,
                    sequenceStart: currentSequence + 1,
                    cancellationToken
                );

                foreach (var chunkInfo in chunkInfos)
                {
                    var data = await _http.GetByteArrayAsync(baseUrl + chunkInfo.Filename);

                    Write(data, 0, data.Length);
                    currentSequence = chunkInfo.Sequence;
                }

                await Task.Delay(chunkInfos.Last().Duration, cancellationToken);
            }
        }

        public async Task<IList<M3UChunkInfo>> GetChunks(string chunklistUrl, int sequenceStart, CancellationToken cancellationToken)
        {
            string chunklist = await _http.GetStringAsync(chunklistUrl, cancellationToken);

            var eligibleChunks = M3UParser.GetChunksFromM3U(chunklist)
                .Where(ch => ch.Sequence >= sequenceStart);

            return eligibleChunks.ToList();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _buffer.Write(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _buffer.Seek(offset, origin);
        }

        public override void Flush() { }
        public override void SetLength(long value) { }
    }
}
