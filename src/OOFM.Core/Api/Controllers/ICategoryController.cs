using OOFM.Core.Api.Models;

namespace OOFM.Core.Api.Controllers;
public interface ICategoryController
{
    Task<IList<Category>> GetAllCategories(CancellationToken cancellationToken = default);
    Task<ExtendedCategory> GetExtendedCategory(Category category, CancellationToken cancellationToken = default);
}
