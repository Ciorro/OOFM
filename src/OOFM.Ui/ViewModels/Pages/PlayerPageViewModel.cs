﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OOFM.Core;
using OOFM.Core.Api.Controllers;
using OOFM.Core.Api.Models;
using OOFM.Core.Settings;
using OOFM.Ui.Factories;
using OOFM.Ui.Navigation;
using OOFM.Ui.Navigation.Attributes;
using OOFM.Ui.Services;
using OOFM.Ui.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace OOFM.Ui.ViewModels.Pages
{
    [PageKey("player")]
    internal partial class PlayerPageViewModel : ObservableObject, INavigationPage
    {
        private readonly IRadioPlayer _radioPlayer;
        private readonly IPlaylistService _playlistService;
        private readonly IStationController _stationController;
        private readonly ICategoryController _categoryController;
        private readonly IStationItemFactory _stationItemFactory;

        private readonly UserProfile _currentUserProfile;


        [ObservableProperty]
        private Station? _currentStation;

        [ObservableProperty]
        private Playlist? _playlist;

        [ObservableProperty]
        private ObservableCollection<StationItemViewModel>? _recommendedStations;

        public PlayerPageViewModel(
            IRadioPlayer radioPlayer,
            IPlaylistService playlistService,
            IStationController stationController,
            ICategoryController categoryController,
            IStationItemFactory stationItemFactory,
            IUserProfileService userProfileService)
        {
            _radioPlayer = radioPlayer;
            _playlistService = playlistService;
            _stationController = stationController;
            _categoryController = categoryController;
            _stationItemFactory = stationItemFactory;

            _currentUserProfile = userProfileService.CurrentUserProfile ?? new();
        }

        public bool IsCurrentSongFavorite
        {
            get
            {
                return _currentUserProfile.FavoriteSongs.Contains(Playlist!.CurrentSong!) == true;
            }
            set
            {
                if (Playlist?.CurrentSong is not null)
                {
                    if (value)
                    {
                        _currentUserProfile.FavoriteSongs.Add(Playlist.CurrentSong);
                    }
                    else
                    {
                        _currentUserProfile.FavoriteSongs.Remove(Playlist.CurrentSong);
                    }
                }
            }
        }

        [RelayCommand]
        private void RecommendationSelected(StationItemViewModel? stationItemViewModel)
        {
            if (stationItemViewModel is not null)
            {
                var station = stationItemViewModel.Station;

                _radioPlayer.Play(station);
                SetStation(station);
            }
        }

        public void OnResumed()
        {
            SetStation(_radioPlayer.CurrentStation);
        }

        public void OnPaused()
        {
            SetStation(null);
        }

        public void SetStation(Station? station)
        {
            if (station?.Id == CurrentStation?.Id)
            {
                return;
            }

            if (CurrentStation is not null)
            {
                _playlistService.Unsubscribe(CurrentStation.Id, OnPlaylistChanged);
            }

            CurrentStation = station;

            if (station is not null)
            {
                _playlistService.Subscribe(station.Id, OnPlaylistChanged);
                Playlist = _playlistService.GetCurrentPlaylist(station.Id);
                UpdateRecommendations(station);
            }
        }

        private void UpdateRecommendations(Station? station)
        {
            RecommendedStations?.Clear();

            if (station is null)
            {
                return;
            }

            Task.Run(async () =>
            {
                var recommendedStationItems = (await GetRecommendations(station)).Select(s =>
                {
                    return _stationItemFactory.Create(s);
                });

                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    RecommendedStations = new ObservableCollection<StationItemViewModel>(recommendedStationItems);
                });
            });
        }

        private async Task<IList<Station>> GetRecommendations(Station station, CancellationToken cancellationToken = default)
        {
            var extStation = await _stationController.GetExtendedStation(station, cancellationToken);
            var recommendedStationsIds = new HashSet<int>();

            foreach (var category in extStation.Categories)
            {
                var extCategory = await _categoryController.GetExtendedCategory(category, cancellationToken);

                foreach (var stationId in extCategory.Stations)
                {
                    if (stationId != station.Id)
                    {
                        recommendedStationsIds.Add(stationId);
                    }
                }
            }

            return await _stationController.GetStationsById(recommendedStationsIds, cancellationToken);
        }

        partial void OnPlaylistChanged(Playlist? value)
        {
            Playlist = value;
            OnPropertyChanged(nameof(IsCurrentSongFavorite));
        }
    }
}
