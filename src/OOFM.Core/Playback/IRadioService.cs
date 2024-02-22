using OOFM.Core.Models;

namespace OOFM.Core.Playback;
public interface IRadioService
{
    event Action<Station>? PlaybackStarted;
    event Action<Station>? PlaybackStopped;

    public Station? CurrentStation { get; }

    void Play(Station slug);
    void Stop();
}
