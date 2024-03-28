using CommunityToolkit.Mvvm.ComponentModel;
using OOFM.Core;
using OOFM.Core.Api.Controllers;
using OOFM.Ui.Attributes;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace OOFM.Ui.ViewModels.Pages;

[PageKey("home")]
internal partial class HomePageViewModel : ObservableObject, INavigationPage
{
    private readonly IStationController _stationController;
    private readonly IRadioPlayer _radioPlayer;
    private readonly IStationItemFactory _stationItemFactory;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _featured;

    [ObservableProperty]
    private ObservableCollection<StationItemViewModel>? _recommended;

    [ObservableProperty]
    private StationItemViewModel? _selectedStation;

    public HomePageViewModel(
        IRadioPlayer radioPlayer,
        IStationController stationController,
        IStationItemFactory stationItemFactory)
    {
        _stationController = stationController;
        _stationItemFactory = stationItemFactory;
        _radioPlayer = radioPlayer;
    }

    public void OnResumed()
    {
        if (_radioPlayer.CurrentStation is not null)
        {
            SelectedStation = _stationItemFactory.Create(_radioPlayer.CurrentStation);
        }

        Task.Run(async () =>
        {
            try
            {
                var stations = await _stationController.GetFeaturedStations();

                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    Featured = new ObservableCollection<StationItemViewModel>(stations.Select(s =>
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
