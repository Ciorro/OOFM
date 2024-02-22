namespace OOFM.Core.Api;
public class HttpClientProvider : IHttpClientProvider, IDisposable
{
    private static HttpClient? _httpClientInstance;

    public HttpClient GetHttpClient()
    {
        return _httpClientInstance ??= new HttpClient();
    }

    public void Dispose()
    {
        _httpClientInstance?.Dispose();
    }
}
