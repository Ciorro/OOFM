using OOFM.Core.Models;

namespace OOFM.Core.Api.Controllers
{
    public interface IPlaylistController
    {
        Task<Playlist> GetPlaylistById(int id, CancellationToken cancellationToken = default);
        Task<IDictionary<int, Playlist>> GetPlaylistsByIds(IEnumerable<int> ids, CancellationToken cancellationToken = default);
        Task<IDictionary<int, Playlist>> GetAllPlaylists(CancellationToken cancellationToken = default);
    }
}
