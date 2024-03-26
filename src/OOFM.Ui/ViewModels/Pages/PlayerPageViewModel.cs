using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Core.Api.Models;
using OOFM.Ui.Attributes;
using OOFM.Ui.Navigation;
using OOFM.Ui.Services;

namespace OOFM.Ui.ViewModels.Pages
{
    [PageKey("player")]
    internal partial class PlayerPageViewModel : ObservableObject, INavigationPage
    {
        private readonly IRadioPlayer _radioPlayer;
        private readonly IPlaylistService _playlistService;

        [ObservableProperty]
        private Station? _station;

        [ObservableProperty]
        private Playlist? _playlist;

        public PlayerPageViewModel(IRadioPlayer radioPlayer, IPlaylistService playlistService)
        {
            _radioPlayer = radioPlayer;
            _playlistService = playlistService;
        }

        public void OnResumed()
        {
            Station = _radioPlayer.CurrentStation;

            if (Station is not null)
            {
                _playlistService.Subscribe(Station.Id, OnPlaylistUpdated);
                Playlist = _playlistService.GetCurrentPlaylist(Station.Id);
            }
        }

        public void OnPaused()
        {
            if (Station is not null)
            {
                _playlistService.Unsubscribe(Station.Id, OnPlaylistUpdated);
            }
        }

        private void OnPlaylistUpdated(Playlist playlist)
        {
            Playlist = playlist;
        }
    }
}
