using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Core.Api.Controllers;
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

        _playlistService.PlaylistsUpdated += OnPlaylistsUpdated;
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

    private void OnPlaylistsUpdated()
    {

    }

    partial void OnSelectedStationChanged(StationItemViewModel? value)
    {
        if (value is null)
        {
            _radioPlayer.Stop();
        }
        else
        {
            _radioPlayer.Play(value.Station!);
        }
    }
}
