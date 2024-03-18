using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Radio;
using OOFM.Ui.ViewModels.Items;

namespace OOFM.Ui.ViewModels;

internal partial class ApplicationViewModel : ObservableObject
{
    private readonly IPageFactory _pageFactory;
    private readonly IRadioService _radioService;
    private readonly IStationItemFactory _stationItemFactory;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isPlaybackEnabled;

    [ObservableProperty]
    private StationItemViewModel? _currentStation;

    public ApplicationViewModel(
        INavigationService navigationService,
        IPageFactory pageFactory,
        IRadioService radioService,
        IStationItemFactory stationItemFactory)
    {
        _pageFactory = pageFactory;
        _radioService = radioService;
        _stationItemFactory = stationItemFactory;

        _radioService.PlaybackStarted += (station) =>
        {
            IsPlaybackEnabled = true;
            CurrentStation = _stationItemFactory.Create(station);
        };
        _radioService.PlaybackStopped += (_) =>
        {
            IsPlaybackEnabled = false;
            CurrentStation = null;
        };
        _radioService.StationRefreshed += (station) =>
        {
            CurrentStation = null;
            CurrentStation = _stationItemFactory.Create(station);
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
