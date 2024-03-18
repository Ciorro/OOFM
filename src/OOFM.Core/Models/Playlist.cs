using System.Text.Json.Serialization;

namespace OOFM.Core.Models
{
    public class Playlist
    {
        [JsonPropertyName("playlist")]
        public IList<Song> Queue { get; set; }
        public Song? CurrentSong { get; set; }

        public Playlist()
        {
            Queue = new List<Song>();
        }
    }
}
