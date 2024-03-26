namespace OOFM.Core.Streaming.M3U;

public class M3UChunkData : IM3UChunk
{
    public int Sequence { get; }
    public TimeSpan Duration { get; }
    public byte[] Data { get; }

    public M3UChunkData(int sequence, TimeSpan duration, byte[] data)
    {
        Sequence = sequence;
        Duration = duration;
        Data = data;
    }
}
