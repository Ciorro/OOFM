using OOFM.Core.Api.Models;

namespace OOFM.Core.Api.Controllers;
public interface IStationController
{
    Task<Station> GetStationById(int id, CancellationToken cancellationToken = default);
    Task<Station> GetStationBySlug(string slug, CancellationToken cancellationToken = default);
    Task<IList<Station>> GetStationsById(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<IList<Station>> GetStationsBySlug(IEnumerable<string> slugs, CancellationToken cancellationToken = default);
    Task<IList<Station>> GetAllStations(CancellationToken cancellationToken = default);
}
