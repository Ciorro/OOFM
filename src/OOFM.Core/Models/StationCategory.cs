namespace OOFM.Core.Models;
public class StationCategory(string slug, string? name = default) : IEquatable<StationCategory>
{
    public string Slug { get; init; } = slug;
    public string? Name { get; init; } = name;

    public bool Equals(StationCategory? other)
    {
        if (other is null)
            return false;
        return other.Slug == Slug;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as StationCategory);
    }

    public override int GetHashCode()
    {
        return Slug.GetHashCode();
    }

    public static bool operator ==(StationCategory s1, StationCategory s2)
    {
        if (s1 is null)
            return s2 is null;
        return s1.Equals(s2);
    }

    public static bool operator !=(StationCategory s1, StationCategory s2)
    {
        return !(s1 == s2);
    }
}
