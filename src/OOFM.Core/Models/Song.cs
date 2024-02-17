using OOFM.Core.Models.Serialization;
using System.Text.Json.Serialization;

namespace OOFM.Core.Models;

[JsonConverter(typeof(SongJsonConverter))]
public class Song : IEquatable<Song>
{
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public string? Cover { get; set; }


    public bool Equals(Song? other)
    {
        if (other is null)
            return false;

        return other.Title == Title && other.Artist == Artist;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Song);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Artist);
    }

    public static bool operator ==(Song s1, Song s2)
    {
        if (s1 is null)
            return s2 is null;

        return s1.Equals(s2);
    }

    public static bool operator !=(Song s1, Song s2)
    {
        return !(s1 == s2);
    }
}
