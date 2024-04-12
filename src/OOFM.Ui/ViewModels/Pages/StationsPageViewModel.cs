using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;
using OOFM.Ui.ViewModels.Items;
using System.Collections.ObjectModel;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("stations-list")]
internal partial class StationsPageViewModel : ObservableObject, INavigationPage
{
    private readonly IRadioPlayer _radioPlayer;
    private readonly IStationDatabase _stationDatabase;
    private readonly IStationItemFactory _stationItemFactory;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _radioStations;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public StationsPageViewModel(
        IRadioPlayer radioPlayer,
        IStationDatabase stationDatabase,
        IStationItemFactory stationItemFactory)
    {
        _stationDatabase = stationDatabase;
        _stationItemFactory = stationItemFactory;
        _radioPlayer = radioPlayer;
    }

    public void OnInitialized()
    {
        RadioStations = new ObservableCollection<StationItemViewModel>(_stationDatabase.Select(s =>
        {
            return _stationItemFactory.Create(s);
        }));
    }

    public void OnResumed()
    {
        if (_radioPlayer.CurrentStation is not null)
        {
            SelectedStation = _stationItemFactory.Create(_radioPlayer.CurrentStation);
        }
    }

    partial void OnSelectedStationChanged(StationItemViewModel? value)
    {
        if (value is null)
        {
            _radioPlayer.Stop();
        }
        else
        {
            _radioPlayer.Play(value.Station!);
        }
    }
}