namespace OOFM.Core.Models;
public class Station : IEquatable<Station>
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string StreamUrl { get; set; }
    public string LogoUrl { get; set; }

    public IList<Song> Playlist { get; init; }
    public IList<StationCategory> Categories { get; init; }

    public Station(string name = "", string slug = "", string streamUrl = "", string logoUrl = "")
    {
        Name = name;
        Slug = slug;
        StreamUrl = streamUrl;
        LogoUrl = logoUrl;

        Playlist = new List<Song>();
        Categories = new List<StationCategory>();
    }

    public Song? CurrentSong => Playlist.FirstOrDefault();

    public bool Equals(Station? other)
    {
        if (other is null)
            return false;
        return other.Slug == Slug;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Station);
    }

    public override int GetHashCode()
    {
        return Slug.GetHashCode();
    }

    public static bool operator ==(Station s1, Station s2)
    {
        if (s1 is null)
            return s2 is null;
        return s1.Equals(s2);
    }

    public static bool operator !=(Station s1, Station s2)
    {
        return !(s1 == s2);
    }
}
