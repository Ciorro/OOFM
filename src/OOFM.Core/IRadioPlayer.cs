using OOFM.Core.Api.Models;

namespace OOFM.Core;

public interface IRadioPlayer
{
    event Action<Station>? PlaybackStarted;
    event Action<Station>? PlaybackStopped;

    Station? CurrentStation { get; }

    void Play(Station station);
    void Stop();
}
