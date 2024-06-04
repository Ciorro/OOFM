using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Core.Services;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;
using OOFM.Ui.ViewModels.Items;
using System.Collections.ObjectModel;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("stations-list")]
internal partial class StationsPageViewModel : ObservableObject, INavigationPage
{
    private readonly IRadioService _radioService;
    private readonly IStationDatabase _stationDatabase;
    private readonly IStationItemFactory _stationItemFactory;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _radioStations;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public StationsPageViewModel(
        IRadioService radioService,
        IStationDatabase stationDatabase,
        IStationItemFactory stationItemFactory)
    {
        _stationDatabase = stationDatabase;
        _stationItemFactory = stationItemFactory;
        _radioService = radioService;
    }

    public void OnInitialized()
    {
        RadioStations = new ObservableCollection<StationItemViewModel>(_stationDatabase.Stations.Select(s =>
        {
            return _stationItemFactory.Create(s);
        }));
    }

    public void OnResumed()
    {
        if (_radioService.CurrentStation is not null)
        {
            SelectedStation = _stationItemFactory.Create(_radioService.CurrentStation);
        }
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