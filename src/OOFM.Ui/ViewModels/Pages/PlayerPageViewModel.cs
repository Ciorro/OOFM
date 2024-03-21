using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core.Models;
using OOFM.Ui.Attributes;
using OOFM.Ui.Navigation;
using OOFM.Ui.Radio;
using OOFM.Ui.Services;

namespace OOFM.Ui.ViewModels.Pages
{
    [PageKey("player")]
    internal partial class PlayerPageViewModel : ObservableObject, INavigationPage
    {
        private readonly IRadioService _radioService;
        private readonly IPlaylistService _playlistService;

        [ObservableProperty]
        private Station? _station;

        [ObservableProperty]
        private Playlist? _playlist;

        public PlayerPageViewModel(IRadioService radioService, IPlaylistService playlistService)
        {
            _radioService = radioService;
            _playlistService = playlistService;
        }

        public void OnResumed()
        {
            Station = _radioService.CurrentStation;

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
