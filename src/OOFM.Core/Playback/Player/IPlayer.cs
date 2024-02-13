namespace OOFM.Core.Playback.Player;

public interface IPlayer
{
    bool IsPlaying { get; }
    float Volume { get; set; }

    void Play();
    void Stop();
}
