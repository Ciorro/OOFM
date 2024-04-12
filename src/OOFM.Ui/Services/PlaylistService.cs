using Microsoft.Extensions.Hosting;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Api.Models;

namespace OOFM.Ui.Services
{
    internal class PlaylistService : BackgroundService, IPlaylistService
    {
        private readonly IPlaylistController _playlistController;
        private readonly HashSet<Subscribtion> _subscribtions = new();

        private IDictionary<int, Playlist>? _playlists;

        public PlaylistService(IPlaylistController playlistController)
        {
            _playlistController = playlistController;
        }

        public void Subscribe(int id, Action<Playlist> action)
        {
            _subscribtions.Add(new Subscribtion(id, action));
        }

        public void Unsubscribe(int id, Action<Playlist> action)
        {
            _subscribtions.Remove(new Subscribtion(id, action));
        }

        public Playlist? GetCurrentPlaylist(int id)
        {
            if (_playlists?.TryGetValue(id, out var playlist) == true)
            {
                return playlist;
            }

            return null;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _playlists = await _playlistController.GetAllPlaylists(stoppingToken);
                    
                    foreach (var subscribtion in _subscribtions)
                    {
                        Notify(subscribtion);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"MESSAGE:\n{e.Message}\n\nHELP:\n{e.HelpLink}\n\nSTACK TRACE:\n{e.StackTrace}");
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        private void Notify(Subscribtion subscribtion)
        {
            var playlist = GetCurrentPlaylist(subscribtion.Id);

            if (playlist is not null)
            {
                subscribtion.Action.Invoke(playlist ?? new());
            }
        }

        record struct Subscribtion
        {
            //TODO: Implement iequatable
            public int Id { get; }
            public Action<Playlist> Action { get; }

            public Subscribtion(int id, Action<Playlist> action)
            {
                Id = id;
                Action = action;
            }
        }
    }
}
