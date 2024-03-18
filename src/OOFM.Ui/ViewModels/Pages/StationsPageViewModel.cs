using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Models;
using OOFM.Ui.Attributes;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Radio;
using OOFM.Ui.ViewModels.Items;
using System.Threading;
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
    private readonly IStationItemFactory _stationItemFactory;
    [ObservableProperty]
    private CollectionView? _radioStations;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public StationsPageViewModel(
        IStationController stationController,
        ICategoryController categoryController,
        IRadioService radioService,
        IStationItemFactory stationItemFactory)
    {
        _stationController = stationController;
        _categoryController = categoryController;
        _radioService = radioService;
        _stationItemFactory = stationItemFactory;

        if (_radioService.CurrentStation is not null)
        {
            SelectedStation = _stationItemFactory.Create(_radioService.CurrentStation);
        }
    }

    public void OnInitialized()
    {
        Task.Run(async () =>
        {
            try
            {
                var stations = await _stationController.GetAllStations();

                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    RadioStations = (CollectionView)CollectionViewSource.GetDefaultView(stations?.Select(s =>
                    {
                        return _stationItemFactory.Create(s);
                    }));
                });
            }
            catch { }
        });
    }

    partial void OnSelectedStationChanged(StationItemViewModel? value)
    {
        if (value is null)
        {
            _radioService.Stop();
        }
        else
        {
            _radioService.Play(value.Station!);
        }
    }
}