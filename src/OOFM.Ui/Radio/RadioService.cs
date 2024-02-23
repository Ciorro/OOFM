using Microsoft.Extensions.Hosting;
using OOFM.Core.Api;
using OOFM.Core.Models;

namespace OOFM.Ui.Radio;

public class RadioService : BackgroundService, IRadioService
{
    const int RefreshInterval = 30000;

    private readonly IStationController _stationController;

    public event Action<Station>? PlaybackStarted;
    public event Action<Station>? PlaybackStopped;
    public event Action<Station>? StationRefreshed;

    public Station? CurrentStation { get; private set; }

    public RadioService(IStationController stationController)
    {
        _stationController = stationController;
    }

    public void Play(Station station)
    {
        ArgumentNullException.ThrowIfNull(station, nameof(station));

        Stop();

        CurrentStation = station;
        PlaybackStarted?.Invoke(CurrentStation);
    }

    public void Stop()
    {
        if (CurrentStation is not null)
        {
            PlaybackStopped?.Invoke(CurrentStation);
            CurrentStation = null;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(RefreshInterval, stoppingToken);

            if (CurrentStation is not null)
            {
                var station = await _stationController.GetSingleStation(CurrentStation.Slug, stoppingToken);

                CurrentStation = station;
                StationRefreshed?.Invoke(station);
            }
        }
    }
}
