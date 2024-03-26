using System.Globalization;

namespace OOFM.Core.Streaming.M3U;

internal static class M3UParser
{
    public static string GetChunklistFilenameFromM3U(string m3uPlaylist)
    {
        foreach (var line in m3uPlaylist.Split('\n'))
        {
            if (!line.StartsWith('#'))
            {
                int paramsIndex = line.IndexOf('?');

                if (paramsIndex >= 0)
                {
                    return line[..paramsIndex];
                }

                return line;
            }
        }

        throw new InvalidDataException("Invalid M3U file.");
    }

    public static IList<M3UChunkInfo> GetChunksFromM3U(string chunklist)
    {
        var chunks = new List<M3UChunkInfo>(5);

        int? sequence = null;
        float? duration = null;

        foreach (var line in chunklist.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            else if (line.StartsWith("#EXT-X-MEDIA-SEQUENCE"))
            {
                sequence = int.Parse(line.Split(':')[1]);
            }
            else if (line.StartsWith("#EXTINF"))
            {
                string args = line.Split(':')[1];
                string arg1 = args.Split(',', StringSplitOptions.RemoveEmptyEntries).First();
                duration = float.Parse(arg1, CultureInfo.InvariantCulture);
            }
            else if (!line.StartsWith('#'))
            {
                if (!sequence.HasValue || !duration.HasValue)
                {
                    throw new InvalidDataException("Invalid M3U file.");
                }

                chunks.Add(new M3UChunkInfo
                (
                    sequence: sequence.Value,
                    duration: TimeSpan.FromSeconds(duration.Value),
                    filename: line
                ));

                sequence++;
                duration = null;
            }
        }

        return chunks;
    }
}
