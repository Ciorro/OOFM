using Microsoft.Extensions.DependencyInjection;

namespace OOFM.Ui.Navigation;

internal class PageFactory : IPageFactory
{
    private readonly IServiceProvider _services;

    public PageFactory(IServiceProvider services)
    {
        _services = services;
    }

    public INavigationPage CreatePage(string pageKey)
    {
        return _services.GetRequiredKeyedService<INavigationPage>(pageKey);
    }
}
