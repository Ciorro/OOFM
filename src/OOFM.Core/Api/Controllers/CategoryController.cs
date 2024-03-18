using OOFM.Core.Models;
using System.Text.Json;

namespace OOFM.Core.Api.Controllers;
public class CategoryController : ICategoryController
{
    private readonly IApiClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public CategoryController(IApiClient client)
    {
        _client = client;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
    {
        var content = await _client.Request($"/radio/categories", cancellationToken);

        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            var categories = json.Deserialize<List<Category>>(_jsonOptions);
            if (categories is null)
            {
                throw new JsonException("Invalid json.");
            }

            return categories.ToList();
        }
    }
}
