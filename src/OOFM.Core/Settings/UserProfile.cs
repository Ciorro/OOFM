using OOFM.Core.Api.Models;

namespace OOFM.Core.Settings;

public class UserProfile 
{
    public HashSet<Song> FavoriteSongs { get; set; } = new();
}
