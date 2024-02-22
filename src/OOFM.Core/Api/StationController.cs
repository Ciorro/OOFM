using OOFM.Core.Models;
using System.Text.Json;

namespace OOFM.Core.Api;
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

    public async Task<IEnumerable<Station>> GetAllStations(CancellationToken cancellationToken)
    {
        var content = await _client.Request($"/radio/stationsCategories", cancellationToken);
        
        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            var stations = json.RootElement.EnumerateArray()
                .SelectMany(category => category.GetProperty("items").EnumerateArray()
                .Select(stationElement => stationElement.Deserialize<Station>(_jsonOptions)!));

            if (stations is null)
            {
                throw new JsonException("Invalid json.");
            }

            return stations.Distinct().ToList();
        }
    }

    public async Task<Station> GetSingleStation(string slug, CancellationToken cancellationToken)
    {
        var content = await _client.Request($"/radio/station/{slug}", cancellationToken);

        using (var ms = new MemoryStream(content))
        {
            var json = await JsonDocument.ParseAsync(ms);

            var station = json.Deserialize<Station>(_jsonOptions)!;
            if (station is null)
            {
                throw new JsonException("Invalid json.");
            }

            var currentSong = json.RootElement.GetProperty("currentSong").Deserialize<Song>(_jsonOptions);
            if (currentSong is null)
            {
                throw new JsonException("Invalid json.");
            }
            station.Playlist.Insert(0, currentSong);

            return station;
        }
    }
}
