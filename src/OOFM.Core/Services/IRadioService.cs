using OOFM.Core.Api.Models;

namespace OOFM.Core.Services;

public interface IRadioService
{
    event Action<Station>? PlaybackStarted;
    event Action<Station>? PlaybackStopped;

    Station? CurrentStation { get; }
    float Volume { get; set; }
    bool IsMuted { get; set; }

    void Play(Station station);
    void Stop();
}
