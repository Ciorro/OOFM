using System;

namespace OOFM.Core.Api;

public class ApiClient(IHttpClientProvider httpClientProvider) : IApiClient
{
    public async Task<byte[]> Request(string path, CancellationToken cancellationToken)
    {
        var client = httpClientProvider.GetHttpClient();

        return await client.GetByteArrayAsync(
            requestUri: GetFullUrl(path),
            cancellationToken: cancellationToken
        );
    }

    private string GetFullUrl(string path)
    {
        if (!path.StartsWith('/'))
            path = path.Insert(0, "/");
        return $"https://open.fm/api{path}";
    }
}
