using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core.Playback;
using OOFM.Ui.Navigation;
using OOFM.Ui.ViewModels.Items;

namespace OOFM.Ui.ViewModels;

internal partial class ApplicationViewModel : ObservableObject
{
    private readonly IPageFactory _pageFactory;
    private readonly IRadioService _radioService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private StationItemViewModel? _currentStation;

    public ApplicationViewModel(INavigationService navigationService, IPageFactory pageFactory, IRadioService radioService)
    {
        _pageFactory = pageFactory;

        _radioService = radioService;
        _radioService.PlaybackStarted += (station) =>
        {
            CurrentStation = new StationItemViewModel(station);
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
}
