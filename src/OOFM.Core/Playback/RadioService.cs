using OOFM.Core.Models;

namespace OOFM.Core.Playback;
public class RadioService : IRadioService
{
    public event Action<Station>? PlaybackStarted;
    public event Action<Station>? PlaybackStopped;

    public Station? CurrentStation { get; private set; }

    public RadioService()
    {
        
    }

    public void Play(Station station)
    {
        ArgumentNullException.ThrowIfNull(station, nameof(station));

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
}
