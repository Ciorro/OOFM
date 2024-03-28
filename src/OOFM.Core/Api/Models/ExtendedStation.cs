namespace OOFM.Core.Api.Models;

public record ExtendedStation : Station
{
    public List<Category> Categories { get; set; } = new();
}
