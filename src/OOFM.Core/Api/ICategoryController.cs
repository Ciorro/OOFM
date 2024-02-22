using OOFM.Core.Models;

namespace OOFM.Core.Api;
public interface ICategoryController
{
    Task<IEnumerable<StationCategory>> GetCategories(CancellationToken cancellationToken = default);
}
