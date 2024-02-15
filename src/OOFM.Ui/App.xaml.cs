using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OOFM.Ui.Extensions;
using OOFM.Ui.Navigation;
using OOFM.Ui.ViewModels;
using OOFM.Ui.Windows;
using System.Windows;

namespace OOFM.Ui;

public partial class App : Application
{
    private readonly IHost _appHost;

    public App()
    {
        _appHost = Host.CreateDefaultBuilder().ConfigureServices(services =>
        {
            services.AddPages();
            services.AddSingleton<IPageFactory, PageFactory>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<ApplicationViewModel>();
            services.AddSingleton(s => new FluentWindow
            {
                DataContext = s.GetRequiredService<ApplicationViewModel>()
            });

        }).Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _appHost.Start();
        _appHost.Services.GetRequiredService<FluentWindow>().Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _appHost.Dispose();
        base.OnExit(e);
    }
}
