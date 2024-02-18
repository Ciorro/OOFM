using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core.Models;

namespace OOFM.Ui.ViewModels;
internal partial class StationItemViewModel(Station station) : ObservableObject
{
    private readonly Station _station = station;

    public string Name => _station.Name;
    public string Logo => _station.LogoUrl;
    public Song? CurrentSong { get; set; } = station.CurrentSong;
}
