namespace OOFM.Core.Streaming.Decoding;

public interface IDecoder
{
    void ProcessInput(byte[] buffer, int offset, int count);
    int ProcessOutput(byte[] buffer, int offset, int count);
}
