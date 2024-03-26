namespace OOFM.Core.Streaming.M3U;

public struct M3UChunkInfo : IM3UChunk
{
    public int Sequence { get; }
    public TimeSpan Duration { get; }
    public string Filename { get; }

    public M3UChunkInfo(int sequence, TimeSpan duration, string filename)
    {
        Sequence = sequence;
        Duration = duration;
        Filename = filename;
    }
}
