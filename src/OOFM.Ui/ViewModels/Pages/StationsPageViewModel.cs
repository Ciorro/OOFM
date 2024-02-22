using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core.Api;
using OOFM.Core.Models;
using OOFM.Core.Playback;
using OOFM.Ui.Attributes;
using OOFM.Ui.Navigation;
using OOFM.Ui.ViewModels.Items;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("stations-list")]
internal partial class StationsPageViewModel : ObservableObject, INavigationPage
{
    private readonly IStationController _stationController;
    private readonly ICategoryController _categoryController;
    private readonly IRadioService _radioService;

    [ObservableProperty]
    private CollectionView? _radioStations;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public StationsPageViewModel(IStationController stationController, ICategoryController categoryController, IRadioService radioService)
    {
        _stationController = stationController;
        _categoryController = categoryController;
        _radioService = radioService;

        if (_radioService.CurrentStation is not null)
        {
            SelectedStation = new StationItemViewModel(_radioService.CurrentStation);
        }
    }

    [RelayCommand]
    private async Task LoadStations(CancellationToken cancellationToken)
    {
        ICollection<Station>? stations = null;

        try
        {
            stations = await FetchStations(default);
        }
        catch (Exception e)
        {
            //TODO: Log
            MessageBox.Show($"MESSAGE:\n{e.Message}\n\nHELP:\n{e.HelpLink}\n\nSTACK TRACE:\n{e.StackTrace}");
        }

        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            RadioStations = (CollectionView)CollectionViewSource.GetDefaultView(stations?.Select(s =>
            {
                return new StationItemViewModel(s);
            }));
        });
    }

    public void OnResumed()
    {
        LoadStationsCommand.ExecuteAsync(null);
    }

    partial void OnSelectedStationChanged(StationItemViewModel? value)
    {
        if (value is null)
        {
            _radioService.Stop();
        }
        else
        {
            _radioService.Play((Station)value);
        }
    }

    private async Task<ICollection<Station>> FetchStations(CancellationToken cancellationToken)
    {
        var stations = await _stationController.GetAllStations(cancellationToken);
        var categories = await _categoryController.GetCategories(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var category in stations.SelectMany(s => s.Categories))
        {
            category.Name = categories.FirstOrDefault(c => c == category)?.Name;
        }

        return stations.ToList();
    }
}