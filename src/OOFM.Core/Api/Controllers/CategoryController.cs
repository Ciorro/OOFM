using OOFM.Core.Api.Models;
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

    public async Task<IList<Category>> GetAllCategories(CancellationToken cancellationToken)
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

    public async Task<ExtendedCategory> GetExtendedCategory(Category category, CancellationToken cancellationToken = default)
    {
        var content = await _client.Request($"/radio/category/{category.Slug}", cancellationToken);

        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            var extCategory = json.Deserialize<ExtendedCategory>(_jsonOptions);
            if (extCategory is null)
            {
                throw new JsonException("Invalid json.");
            }

            return extCategory;
        }
    }
}
