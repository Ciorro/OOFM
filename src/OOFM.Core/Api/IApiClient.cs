namespace OOFM.Core.Api;
public interface IApiClient
{
    Task<byte[]> Request(string path, CancellationToken cancellationToken = default);
}
