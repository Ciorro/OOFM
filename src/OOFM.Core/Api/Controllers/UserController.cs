using System.Text.Json;

namespace OOFM.Core.Api.Controllers;

public class UserController : IUserController
{
    private readonly IApiClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public UserController(IApiClient apiClient)
    {
        _client = apiClient;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<string> AppendToken(string url, CancellationToken cancellationToken)
    {
        string requestUrl = "/user/token?fp=" + url;
        var content = await _client.Request(requestUrl, cancellationToken);

        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            if (json.RootElement.TryGetProperty("url", out var urlProperty))
            {
                return urlProperty.GetString() ?? throw new ApiException("Url property value was invalid");
            }
        }

        throw new ApiException("Url property could not be found.");
    }
}
