using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core.Api.Models;
using OOFM.Core.Settings;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;
using System.ComponentModel;
using System.Windows.Data;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("local-music")]
internal partial class MyMusicPageViewModel : ObservableObject, INavigationPage
{
    private readonly UserProfile _currentUserProfile;

    [ObservableProperty]
    private ICollectionView? _favoriteSongs;

    public MyMusicPageViewModel(IUserProfileService userProfileService)
    {
        _currentUserProfile = userProfileService.CurrentUserProfile;
    }

    public string SongFilter
    {
        set
        {
            FavoriteSongs!.Filter = GetSongFilter(value ?? "");
        }
    }

    public void OnResumed()
    {
        FavoriteSongs = CollectionViewSource.GetDefaultView(_currentUserProfile.FavoriteSongs);
    }

    public void OnPaused()
    {
        FavoriteSongs = null;
    }

    [RelayCommand]
    void RemoveSong(Song song)
    {
        if (_currentUserProfile?.FavoriteSongs.Remove(song) == true)
        {
            FavoriteSongs?.Refresh();
        }
    }

    private Predicate<object> GetSongFilter(string filter)
    {
        return (obj) =>
        {
            var song = (obj as Song)!;

            return song.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase) ||
                   song.Artist.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
        };
    }
}