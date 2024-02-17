using OOFM.Core.Models;
using System.Text.Json;

namespace OOFM.Core.Api;

public class ApiClient : IApiClient, IDisposable
{
    const string BaseAddress = "http://www.open.fm";

    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
        _http.BaseAddress = new Uri(BaseAddress);
    }

    public async Task<IEnumerable<Station>> GetAllStations()
    {
        var content = await Request($"/api/radio/stationsCategories");
        var json = await JsonDocument.ParseAsync(content.ReadAsStream());

        var stations = json.RootElement.EnumerateArray()
            .SelectMany(category => category.GetProperty("items").EnumerateArray())
            .Select(stationElement => stationElement.Deserialize<Station>(_jsonOptions));

        if (stations is null)
        {
            throw new JsonException("Invalid json.");
        }

        return stations!;
    }

    public async Task<Station> GetSingleStation(string slug)
    {
        var content = await Request($"/api/radio/station/{slug}");
        var json = await JsonDocument.ParseAsync(content.ReadAsStream());

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

    public async Task<IEnumerable<StationCategory>> GetCategories()
    {
        var content = await Request($"/api/radio/categories");
        var json = await JsonDocument.ParseAsync(content.ReadAsStream());

        var categories = json.Deserialize<List<StationCategory>>(_jsonOptions);
        if (categories is null)
        {
            throw new JsonException("Invalid json.");
        }
        return categories;
    }

    private async Task<HttpContent> Request(string endpoint)
    {
        return (await _http.GetAsync(endpoint)).EnsureSuccessStatusCode().Content;
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}
