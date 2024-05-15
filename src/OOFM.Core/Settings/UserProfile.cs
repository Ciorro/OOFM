using OOFM.Core.Api.Models;

namespace OOFM.Core.Settings;

public class UserProfile 
{
    public event Action<Theme>? OnThemeChanged;

    public HashSet<Song> FavoriteSongs { get; set; } = new();
    public float Volume { get; set; } = 1f;
    public bool IsMuted { get; set; }


    private Theme _theme = Theme.Auto;
    public Theme Theme
    {
        get => _theme;
        set
        {
            if (value != _theme)
            {
                OnThemeChanged?.Invoke(value);
            }
            _theme = value;
        }
    }
}
