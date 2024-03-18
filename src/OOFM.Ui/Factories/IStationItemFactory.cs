using OOFM.Core.Models;
using OOFM.Ui.ViewModels.Items;

namespace OOFM.Ui.Factories
{
    internal interface IStationItemFactory
    {
        StationItemViewModel Create(Station station);
    }
}
