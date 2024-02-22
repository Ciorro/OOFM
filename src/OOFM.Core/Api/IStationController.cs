using OOFM.Core.Models;

namespace OOFM.Core.Api;
public interface IStationController
{
    Task<Station> GetSingleStation(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Station>> GetAllStations(CancellationToken cancellationToken = default);
}
