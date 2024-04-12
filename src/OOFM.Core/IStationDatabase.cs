using OOFM.Core.Api.Models;

namespace OOFM.Core;

public interface IStationDatabase : ICollection<Station>
{
    Station? GetStationById(int id);
    Station? GetStationBySlug(string slug);
    IList<Station> GetStationsById(params int[] ids);
    IList<Station> GetStationsBySlug(params string[] slugs);
}
