namespace OOFM.Core.Api;
public class HttpClientProvider : IHttpClientProvider, IDisposable
{
    private static HttpClient? _httpClientInstance;

    public HttpClient GetHttpClient()
    {
        if (_httpClientInstance is null)
        {
            _httpClientInstance = new HttpClient();
            _httpClientInstance.DefaultRequestHeaders.Add("User-Agent", "TEST");
        }

        return _httpClientInstance;
    }

    public void Dispose()
    {
        _httpClientInstance?.Dispose();
    }
}
