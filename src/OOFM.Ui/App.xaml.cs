using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OOFM.Core;
using OOFM.Core.Api;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Settings;
using OOFM.Core.Services;
using OOFM.Ui.Extensions;
using OOFM.Ui.Factories;
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
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddSingleton<IStationController, StationController>();
            services.AddSingleton<ICategoryController, CategoryController>();
            services.AddSingleton<IPlaylistController, PlaylistController>();
            services.AddSingleton<IUserController, UserController>();

            services.AddSingleton<IRadioService, RadioService>();
            services.AddSingleton<IStationDatabase, StationDatabase>();
            services.AddSingleton<IUserProfileService, OSUserProfileService>();
            services.AddHostedService<IPlaylistService, PlaylistService>();

            services.AddPages();
            services.AddSingleton<IPageFactory, PageFactory>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddStationItemFactory();

            services.AddSingleton<ApplicationViewModel>();
            services.AddSingleton(s => new FluentWindow
            {
                DataContext = s.GetRequiredService<ApplicationViewModel>()
            });

        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        _appHost.Start();

        LoadUserProfile();
        await LoadStations();
        await LoadPlaylist();

        _appHost.Services.GetRequiredService<FluentWindow>().Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        SaveUserProfile();

        _appHost.Dispose();
        base.OnExit(e);
    }

    private async Task LoadStations()
    {
        var stationController = _appHost.Services.GetRequiredService<IStationController>();
        var stations = await stationController.GetAllStations();

        var stationDb = _appHost.Services.GetRequiredService<IStationDatabase>();
        foreach (var station in stations)
        {
            stationDb.AddStation(station);
        }
    }

    private async Task LoadPlaylist()
    {
        var playlistController = _appHost.Services.GetRequiredService<IPlaylistController>();
        var playlist = await playlistController.GetAllPlaylists();

        var playlistService = _appHost.Services.GetRequiredService<IPlaylistService>();
        playlistService.Playlist = playlist;
    }

    private void LoadUserProfile()
    {
        _appHost.Services.GetRequiredService<IUserProfileService>().LoadUserProfile();
    }

    private void SaveUserProfile()
    {
        _appHost.Services.GetRequiredService<IUserProfileService>().SaveUserProfile();
    }
}
