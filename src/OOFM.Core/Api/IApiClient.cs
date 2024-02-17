using OOFM.Core.Models;

namespace OOFM.Core.Api;
public interface IApiClient
{
    Task<Station> GetSingleStation(string slug);
    Task<IEnumerable<Station>> GetAllStations();
    Task<IEnumerable<StationCategory>> GetCategories();
}
