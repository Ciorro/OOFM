namespace OOFM.Core.Streaming.Playback;

public interface IPlayer
{
    bool IsPlaying { get; }
    float Volume { get; set; }

    void Play();
    void Stop();
}
