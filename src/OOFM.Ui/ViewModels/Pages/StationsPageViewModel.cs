using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core;
using OOFM.Core.Api.Controllers;
using OOFM.Ui.Attributes;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Services;
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
    private readonly IRadioPlayer _radioPlayer;
    private readonly IStationItemFactory _stationItemFactory;

    [ObservableProperty]
    private CollectionView? _radioStations;

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

        if (_radioPlayer.CurrentStation is not null)
        {
            SelectedStation = _stationItemFactory.Create(_radioPlayer.CurrentStation);
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
            _radioPlayer.Stop();
        }
        else
        {
            _radioPlayer.Play(value.Station!);
        }
    }
}