using OOFM.Core.Api;
using OOFM.Core.Models;

namespace OOFM.Ui.Caching;
internal class StationCache : CacheBase<string, Station>
{
    private readonly IStationController _stationController;

    public StationCache(IStationController stationController)
    {
        _stationController = stationController;
        PersistanceTime = TimeSpan.FromSeconds(30);
    }

    protected async override Task<Station> ObtainValue(string key, CancellationToken cancellationToken = default)
    {
        //TODO: use cancellation token
        return await _stationController.GetSingleStation(key);
    }
}
