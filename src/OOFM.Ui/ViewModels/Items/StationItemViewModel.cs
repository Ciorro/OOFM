using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core.Api.Models;
using OOFM.Core.Services;

namespace OOFM.Ui.ViewModels.Items;

internal partial class StationItemViewModel : ObservableObject, IDisposable, IEquatable<StationItemViewModel>
{
    private readonly IPlaylistService _playlistService;

    [ObservableProperty]
    private Playlist? _playlist;

    public string Name => Station?.Name ?? "";
    public string Logo => Station?.LogoUrl ?? "/Resources/icon.png";

    public StationItemViewModel(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
        _playlistService.PlaylistUpdated += OnPlaylistUpdated;
    }

    private Station? _station;
    public Station Station
    {
        get => _station ?? new Station();
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _station ??= value;

            OnPlaylistUpdated();
        }
    }

    private void OnPlaylistUpdated()
    {
        Playlist = _playlistService.GetPlaylist(Station.Id);
    }

    public void Dispose()
    {
        _playlistService.PlaylistUpdated -= OnPlaylistUpdated;
    }

    #region Equatable

    public bool Equals(StationItemViewModel? other)
    {
        if (other is null)
            return false;
        return Station?.Equals(other.Station) == true;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as StationItemViewModel);
    }

    public override int GetHashCode()
    {
        return Station?.GetHashCode() ?? 0;
    }

    public static bool operator ==(StationItemViewModel s1, StationItemViewModel s2)
    {
        if (s1 is null)
            return s2 is null;
        return s1.Equals(s2);
    }

    public static bool operator !=(StationItemViewModel s1, StationItemViewModel s2)
    {
        return !(s1 == s2);
    }

    #endregion
}
