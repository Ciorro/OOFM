using OOFM.Core.Api.Models;
namespace OOFM.Core.Api.Controllers;

public interface IPlaylistController
{
    Task<IDictionary<int, Playlist>> GetAllPlaylists(CancellationToken cancellationToken = default);
}
