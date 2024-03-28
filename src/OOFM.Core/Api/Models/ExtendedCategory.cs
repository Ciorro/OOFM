using System.Text.Json.Serialization;

namespace OOFM.Core.Api.Models;

public record ExtendedCategory : Category
{

    [JsonPropertyName("items")]
    public List<int> Stations { get; set; } = new();
}
