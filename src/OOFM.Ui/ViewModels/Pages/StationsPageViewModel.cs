using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Core.Api.Controllers;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;
using OOFM.Ui.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("stations-list")]
internal partial class StationsPageViewModel : ObservableObject, INavigationPage
{
    private readonly IStationController _stationController;
    private readonly IRadioPlayer _radioPlayer;
    private readonly IStationItemFactory _stationItemFactory;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _radioStations;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public StationsPageViewModel(
        IRadioPlayer radioPlayer,
        IStationController stationController,
        IStationItemFactory stationItemFactory)
    {
        _stationController = stationController;
        _stationItemFactory = stationItemFactory;
        _radioPlayer = radioPlayer;
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
                    RadioStations = new ObservableCollection<StationItemViewModel>(stations.Select(s =>
                    {
                        return _stationItemFactory.Create(s);
                    }));
                });
            }
            catch { }
        });
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