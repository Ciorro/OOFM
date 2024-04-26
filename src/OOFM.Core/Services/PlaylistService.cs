using Microsoft.Extensions.Hosting;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Api.Models;

namespace OOFM.Core.Services;

public class PlaylistService : BackgroundService, IPlaylistService
{
    public event Action? PlaylistUpdated;

    private readonly IPlaylistController _playlistController;
    private readonly object _lock = new object();

    public PlaylistService(IPlaylistController playlistController)
    { 
        _playlistController = playlistController;
        _playlist = new Dictionary<int, Playlist>();
    }

    private IDictionary<int, Playlist> _playlist;
    public IDictionary<int, Playlist> Playlist
    {
        get => _playlist;
        set
        {
            lock (_lock)
            {
                _playlist = value;
            }
        }
    }

    public Playlist? GetPlaylist(int id)
    {
        lock (_lock)
        {
            if (Playlist?.TryGetValue(id, out var playlist) == true)
            {
                return playlist;
            }
        }

        return null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(15));

            try
            {
                Playlist = await _playlistController.GetAllPlaylists(stoppingToken);
                PlaylistUpdated?.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine($"MESSAGE:\n{e.Message}\n\nHELP:\n{e.HelpLink}\n\nSTACK TRACE:\n{e.StackTrace}");
            }
        }
    }
}
