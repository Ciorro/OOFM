using OOFM.Core.Models;

namespace OOFM.Ui.Services
{
    internal interface IPlaylistService
    {
        void Subscribe(int id, Action<Playlist> action);
        void Unsubscribe(int id, Action<Playlist> action);
        Playlist? GetCurrentPlaylist(int id);
    }
}
