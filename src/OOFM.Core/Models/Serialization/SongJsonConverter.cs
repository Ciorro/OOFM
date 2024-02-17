using System.Text.Json;
using System.Text.Json.Serialization;

namespace OOFM.Core.Models.Serialization;
public class SongJsonConverter : JsonConverter<Song>
{
    public override Song? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int initialDepth = reader.CurrentDepth;

        Song output = new Song();
        string propertyName = "";
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == initialDepth)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propertyName = reader.GetString()!;
                continue;
            }

            switch (propertyName)
            {
                case "title":
                    output.Title = reader.GetString();
                    break;
                case "cover":
                    output.Cover = reader.GetString();
                    break;
                case "artist":
                    output.Artist = reader.GetString();
                    break;
                case "artists":
                    output.Artist = ReadArtistsArray(ref reader);
                    break;
                case "album":
                    output.Album = ReadAlbum(ref reader);
                    break;
            }

            propertyName = "";
        }

        return output;
    }

    public override void Write(Utf8JsonWriter writer, Song value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("title", value.Title);
        writer.WriteString("artist", value.Artist);
        writer.WriteString("album", value.Album);
        writer.WriteString("cover", value.Cover);
        writer.WriteEndObject();
    }

    private string? ReadArtistsArray(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            return null;

        var artists = new List<string?>();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            reader.Read();

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Read();

                if (reader.TokenType != JsonTokenType.PropertyName)
                    continue;
                if (reader.GetString() != "name")
                    continue;

                reader.Read();

                if (reader.TokenType != JsonTokenType.String)
                    continue;

                artists.Add(reader.GetString());
            }
        }

        return string.Join(" & ", artists.Where(a => a is not null));
    }

    private string? ReadAlbum(ref Utf8JsonReader reader)
    {
        //If album is a simple string
        if (reader.TokenType == JsonTokenType.String)
            return reader.GetString();

        //If album isn't a simple string and it isn't a object
        if (reader.TokenType != JsonTokenType.StartObject)
            return null;

        reader.Read();

        //Parse title property name
        if (reader.TokenType != JsonTokenType.PropertyName)
            return null;
        if (reader.GetString() != "title")
            return null;

        reader.Read();

        //Read the album name
        if (reader.TokenType != JsonTokenType.String)
            return null;

        return reader.GetString();
    }
}
