using OOFM.Core.Api.Models;
using OOFM.Ui.ViewModels.Items;

namespace OOFM.Ui.Factories
{
    internal class StationItemFactory : IStationItemFactory
    {
        private readonly IAbstractFactory<StationItemViewModel> _factory;

        public StationItemFactory(IAbstractFactory<StationItemViewModel> factory)
        {
            _factory = factory;
        }

        public StationItemViewModel Create(Station station)
        {
            var item = _factory.Create();
            item.Station = station;
            return item;
        }
    }
}
