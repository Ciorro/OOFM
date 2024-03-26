using OOFM.Core.Api.Models;

namespace OOFM.Core.Api.Controllers;
public interface ICategoryController
{
    Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken = default);
}
