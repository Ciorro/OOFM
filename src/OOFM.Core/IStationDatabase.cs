using OOFM.Core.Api.Models;

namespace OOFM.Core;

public interface IStationDatabase : IEnumerable<Station>
{
    void AddStation(Station station);
    bool RemoveStation(Station station);
    void Clear();

    Station? GetStationById(int id);
    Station? GetStationBySlug(string slug);
    IList<Station> GetStationsById(params int[] ids);
    IList<Station> GetStationsBySlug(params string[] slugs);
}
