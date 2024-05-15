using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core.Settings;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("settings")]
internal partial class SettingsPageViewModel : ObservableObject, INavigationPage
{
    private readonly IUserProfileService _userProfileService;

    [ObservableProperty]
    private Theme _theme;

    public SettingsPageViewModel(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
        _theme = _userProfileService.CurrentUserProfile.Theme;
    }

    void OnPaused()
    {
        _userProfileService.SaveUserProfile();
    }

    partial void OnThemeChanged(Theme value)
    {
        _userProfileService.CurrentUserProfile.Theme = value;
    }
}
