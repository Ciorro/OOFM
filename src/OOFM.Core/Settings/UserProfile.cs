using OOFM.Core.Api.Models;

namespace OOFM.Core.Settings;

public class UserProfile 
{
    public HashSet<Song> FavoriteSongs { get; set; } = new();
    public float Volume { get; set; } = 1f;
    public bool IsMuted { get; set; }
}
