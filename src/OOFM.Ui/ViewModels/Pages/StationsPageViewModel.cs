using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core.Api;
using OOFM.Core.Models;
using OOFM.Ui.Attributes;
using OOFM.Ui.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Windows.Data;
using System.Windows.Threading;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("stations-list")]
internal partial class StationsPageViewModel : ObservableObject, INavigationPage
{
    private readonly IApiClient _api;

    [ObservableProperty]
    private CollectionView? _radioStations;

    public StationsPageViewModel(IApiClient api)
    {
        _api = api;
    }

    public void OnInitialized()
    {
        Task.Run(async () =>
        {
            RadioStations = null;

            var stations = await _api.GetAllStations();
            var categories = await _api.GetCategories();

            foreach (var category in stations.SelectMany(s => s.Categories))
            {
                category.Name = categories.FirstOrDefault(c => c == category)?.Name;
            }

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                RadioStations = (CollectionView)CollectionViewSource.GetDefaultView(stations.Select(s =>
                {
                    return new StationItemViewModel(s);
                }));
            }, DispatcherPriority.SystemIdle);
        });
    }
}