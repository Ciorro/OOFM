using System.Text.Json.Serialization;

namespace OOFM.Core.Api.Models;
public record Station
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Slug { get; init; }
    public string? StreamUrl { get; init; }
    public string? LogoUrl { get; init; }

    [JsonPropertyName("premium")]
    public bool IsPremium { get; init; }
}
