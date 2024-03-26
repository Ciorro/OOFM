using OOFM.Core.Models;

namespace OOFM.Core.Streaming;

public interface IRadioPlayer
{
    event Action<Station>? PlaybackStarted;
    event Action<Station>? PlaybackStopped;

    Station? CurrentStation { get; }

    void Play(Station station);
    void Stop();
}
