using OOFM.Core.Models;

namespace OOFM.Ui.Radio;
public interface IRadioService
{
    event Action<Station>? PlaybackStarted;
    event Action<Station>? PlaybackStopped;
    event Action<Station>? StationRefreshed;

    Station? CurrentStation { get; }

    void Play(Station slug);
    void Stop();
}
