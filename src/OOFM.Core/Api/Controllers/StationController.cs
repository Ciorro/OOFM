using OOFM.Core.Api.Models;
using System.Text.Json;

namespace OOFM.Core.Api.Controllers;

public class StationController : IStationController
{
    private readonly IApiClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public StationController(IApiClient client)
    {
        _client = client;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<Station> GetStationById(int id, CancellationToken cancellationToken = default)
    {
        return (await GetAllStations(cancellationToken)).First(s => s.Id == id);
    }

    public async Task<Station> GetStationBySlug(string slug, CancellationToken cancellationToken = default)
    {
        return (await GetAllStations(cancellationToken)).First(s => s.Slug == slug);
    }

    public async Task<IList<Station>> GetStationsById(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return (await GetAllStations(cancellationToken)).Where(s => ids.Contains(s.Id)).ToList();
    }

    public async Task<IList<Station>> GetStationsBySlug(IEnumerable<string> slugs, CancellationToken cancellationToken = default)
    {
        return (await GetAllStations(cancellationToken)).Where(s => slugs.Contains(s.Slug)).ToList();
    }

    public async Task<IList<Station>> GetFeaturedStations(CancellationToken cancellationToken = default)
    {
        var featuredJson = await _client.Request("/radio/featured", cancellationToken);
        var featuredIds = JsonSerializer.Deserialize<int[]>(featuredJson) ?? [];

        return await GetStationsById(featuredIds, cancellationToken);
    }

    public async Task<IList<Station>> GetAllStations(CancellationToken cancellationToken)
    {
        var content = await _client.Request($"/radio/stations", cancellationToken);

        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            var stations = json.RootElement.EnumerateObject().Select(jObj =>
            {
                return jObj.Value.Deserialize<Station>(_jsonOptions)!;
            }).Where(s => s is not null);

            return stations.ToList();
        }
    }

    public async Task<ExtendedStation> GetExtendedStation(Station station, CancellationToken cancellationToken = default)
    {
        var content = await _client.Request($"/radio/station/{station.Slug}", cancellationToken);

        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            var extStation = json.Deserialize<ExtendedStation>(_jsonOptions);
            if (extStation is null)
            {
                throw new JsonException("Invalid json.");
            }

            return extStation;
        }
    }
}
