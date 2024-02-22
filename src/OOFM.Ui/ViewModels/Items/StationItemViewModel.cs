using OOFM.Core.Models;

namespace OOFM.Ui.ViewModels.Items;
internal partial class StationItemViewModel : ItemViewModel<Station>
{
    public string Name => Item.Name;
    public string Logo => Item.LogoUrl;
    public Song? CurrentSong { get; set; }

    public StationItemViewModel(Station station)
        : base(station)
    {
        CurrentSong = Item.CurrentSong;
    }
}
