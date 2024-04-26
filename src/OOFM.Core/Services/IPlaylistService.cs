using OOFM.Core.Api.Models;

namespace OOFM.Core.Services;

public interface IPlaylistService
{
    event Action PlaylistUpdated;
    IDictionary<int, Playlist> Playlist { get; set; }
    Playlist? GetPlaylist(int id);
}
