namespace OOFM.Core.Api.Models;

public record Song
{
    public string Title { get; set; }
    public string Artist { get; set; }

    public Song(string title, string artist)
    {
        Title = title;
        Artist = artist;
    }
}
