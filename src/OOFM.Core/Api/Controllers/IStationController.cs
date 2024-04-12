using OOFM.Core.Api.Models;

namespace OOFM.Core.Api.Controllers;
public interface IStationController
{
    Task<IList<Station>> GetAllStations(CancellationToken cancellationToken = default);
    Task<int[]> GetFeaturedStations(CancellationToken cancellationToken = default);
    Task<ExtendedStation> GetExtendedStation(Station station, CancellationToken cancellationToken = default);
}
