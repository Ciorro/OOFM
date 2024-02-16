using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Ui.Navigation;
using System.Windows;

namespace OOFM.Ui.ViewModels;

internal partial class ApplicationViewModel : ObservableObject
{
    private readonly IPageFactory _pageFactory;
    private readonly INavigationService _navigationService;

    public ApplicationViewModel(INavigationService navigationService, IPageFactory pageFactory)
    {
        _pageFactory = pageFactory;

        _navigationService = navigationService;
        _navigationService.Navigated += (_) =>
        {
            OnPropertyChanged(nameof(CurrentPage));
        };
    }

    public INavigationPage? CurrentPage
    {
        get => _navigationService.CurrentPage;
    }

    [RelayCommand]
    private void Navigate(string pageKey)
    {
        //_navigationService.Navigate(_pageFactory.CreatePage(pageKey));
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
