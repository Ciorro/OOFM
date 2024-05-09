using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core.Services;
using OOFM.Core.Settings;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.ViewModels.Items;

namespace OOFM.Ui.ViewModels;

internal partial class ApplicationViewModel : ObservableObject
{
    private readonly IPageFactory _pageFactory;
    private readonly IRadioService _radioService;
    private readonly IStationItemFactory _stationItemFactory;
    private readonly INavigationService _navigationService;
    private readonly UserProfile _currentUserProfile;

    [ObservableProperty]
    private StationItemViewModel? _currentStation;

    public ApplicationViewModel(
        IPageFactory pageFactory,
        IRadioService radioService,
        INavigationService navigationService,
        IStationItemFactory stationItemFactory,
        IUserProfileService userProfileService)
    {
        _pageFactory = pageFactory;
        _radioService = radioService;
        _stationItemFactory = stationItemFactory;

        _currentUserProfile = userProfileService.CurrentUserProfile ?? new();
        Volume = _currentUserProfile.Volume;
        IsMuted = _currentUserProfile.IsMuted;

        _radioService.PlaybackStarted += (station) =>
        {
            CurrentStation = _stationItemFactory.Create(station);
        };
        _radioService.PlaybackStopped += (_) =>
        {
            CurrentStation = null;
        };

        _navigationService = navigationService;
        _navigationService.Navigated += (_) =>
        {
            OnPropertyChanged(nameof(CurrentPage));
        };

        _navigationService.Navigate(_pageFactory.CreatePage("home"));
    }

    public INavigationPage? CurrentPage
    {
        get => _navigationService.CurrentPage;
    }

    public float Volume
    {
        get => _radioService.Volume;
        set
        {
            _radioService.Volume = value;
            _currentUserProfile.Volume = value;
        }
    }

    public bool IsMuted
    {
        get => _radioService.IsMuted;
        set
        {
            _radioService.IsMuted = value;
            _currentUserProfile.IsMuted = value;
        }
    }

    [RelayCommand]
    private void Navigate(string pageKey)
    {
        _navigationService.Navigate(_pageFactory.CreatePage(pageKey));
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navigationService.Back();
    }

    [RelayCommand]
    private void NavigateNext()
    {
        _navigationService.Next();
    }

    [RelayCommand]
    private void Radiostop()
    {
        _radioService.Stop();
    }
}
