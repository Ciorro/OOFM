namespace OOFM.Core.Streaming.M3U;

public interface IM3UChunk
{
    int Sequence { get; }
    TimeSpan Duration { get; }
}
