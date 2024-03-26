using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core.Streaming;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Services;
using OOFM.Ui.ViewModels.Items;

namespace OOFM.Ui.ViewModels;

internal partial class ApplicationViewModel : ObservableObject
{
    private readonly IPageFactory _pageFactory;
    private readonly IRadioPlayer _radioPlayer;
    private readonly IStationItemFactory _stationItemFactory;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isPlaybackEnabled;

    [ObservableProperty]
    private StationItemViewModel? _currentStation;

    public ApplicationViewModel(
        IPageFactory pageFactory,
        IRadioPlayer radioPlayer,
        INavigationService navigationService,
        IStationItemFactory stationItemFactory)
    {
        _pageFactory = pageFactory;
        _radioPlayer = radioPlayer;
        _stationItemFactory = stationItemFactory;

        _radioPlayer.PlaybackStarted += (station) =>
        {
            IsPlaybackEnabled = true;
            CurrentStation = _stationItemFactory.Create(station);
        };
        _radioPlayer.PlaybackStopped += (_) =>
        {
            IsPlaybackEnabled = false;
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
    private void PlaybackToggled(bool isEnabled)
    {
        Console.WriteLine(isEnabled);
    }
}
