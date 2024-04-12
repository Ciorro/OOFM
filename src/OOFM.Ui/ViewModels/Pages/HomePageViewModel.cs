using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Api.Models;
using OOFM.Core.Settings;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;
using OOFM.Ui.Services;
using OOFM.Ui.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("home")]
internal partial class HomePageViewModel : ObservableObject, INavigationPage
{
    private readonly IRadioPlayer _radioPlayer;
    private readonly IStationDatabase _stationDatabase;
    private readonly IStationController _stationController;
    private readonly IStationItemFactory _stationItemFactory;
    private readonly IPlaylistService _playlistService;

    private readonly UserProfile _currentUserProfile;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _featuredStations;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _recommendedStations;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public HomePageViewModel(
        IRadioPlayer radioPlayer,
        IStationDatabase stationDatabase,
        IStationController stationController,
        IStationItemFactory stationItemFactory,
        IPlaylistService playlistService,
        IUserProfileService userProfileService)
    {
        _radioPlayer = radioPlayer;
        _stationDatabase = stationDatabase;
        _stationController = stationController;
        _stationItemFactory = stationItemFactory;
        _playlistService = playlistService;

        _currentUserProfile = userProfileService.CurrentUserProfile ?? new();
    }

    public void OnResumed()
    {
        if (_radioPlayer.CurrentStation is not null)
        {
            SelectedStation = _stationItemFactory.Create(_radioPlayer.CurrentStation);
        }

        Task.Run(async () =>
        {
            try
            {
                await RefreshFeaturedStations();
                RefreshRecommendedStations();
            }
            catch { }
        });
    }

    private async Task RefreshFeaturedStations()
    {
        var stationIds = await _stationController.GetFeaturedStations();
        var stations = _stationDatabase.GetStationsById(stationIds);

        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            FeaturedStations = new ObservableCollection<StationItemViewModel>(stations.Select(s =>
            {
                return _stationItemFactory.Create(s);
            }));
        });
    }

    private void RefreshRecommendedStations()
    {
        var recommendedStations = _stationDatabase.Where(station =>
        {
            var playlist = _playlistService.GetCurrentPlaylist(station.Id);

            if (_currentUserProfile.FavoriteSongs.Contains(playlist?.CurrentSong!) ||
                _currentUserProfile.FavoriteSongs.Overlaps(playlist?.Queue!))
            {
                return true;
            }

            return false;
        });

        RecommendedStations = new ObservableCollection<StationItemViewModel>(recommendedStations.Select(s =>
        {
            return _stationItemFactory.Create(s);
        }));
    }

    partial void OnSelectedStationChanged(StationItemViewModel? oldValue, StationItemViewModel? newValue)
    {
        if (newValue is null)
        {
            // If the station disappeared from the recommended list, don't stop it.
            if (RecommendedStations?.Contains(oldValue!) == true)
            {
                _radioPlayer.Stop();
            }
        }
        else
        {
            _radioPlayer.Play(newValue.Station!);
        }
    }
}
