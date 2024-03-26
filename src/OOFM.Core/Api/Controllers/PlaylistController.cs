using OOFM.Core.Api.Models;
using System.Text.Json;

namespace OOFM.Core.Api.Controllers
{
    public class PlaylistController : IPlaylistController
    {
        private readonly IApiClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public PlaylistController(IApiClient apiClient)
        {
            _client = apiClient;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<Playlist> GetPlaylistById(int id, CancellationToken cancellationToken = default)
        {
            return (await GetAllPlaylists(cancellationToken))[id];
        }

        public async Task<IDictionary<int, Playlist>> GetPlaylistsByIds(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            return (await GetAllPlaylists(cancellationToken)).Where(p => ids.Contains(p.Key)).ToDictionary();
        }

        public async Task<IDictionary<int, Playlist>> GetAllPlaylists(CancellationToken cancellationToken = default)
        {
            var content = await _client.Request($"/radio/playlist", cancellationToken);

            using (var ms = new MemoryStream(content))
            {
                var json = await JsonDocument.ParseAsync(ms);
                var playlists = new Dictionary<int, Playlist>();

                foreach (var jObj in json.RootElement.EnumerateObject())
                {
                    if (int.TryParse(jObj.Name, out var id))
                    {
                        playlists.Add(id, jObj.Value.Deserialize<Playlist>(_jsonOptions)!);
                    }
                }

                return playlists;
            }
        }
    }
}
