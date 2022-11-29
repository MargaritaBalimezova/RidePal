﻿using Amazon.S3.Model.Internal.MarshallTransformations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class PlaylistServices : IPlaylistServices
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly ITrackServices trackServices;
        private readonly IPixabayServices pixabayServices;

        public PlaylistServices(RidePalContext context, IMapper mapper, IGenreService genreService, ITrackServices trackServices, IPixabayServices pixabayServices)
        {
            this.db = context;
            this.mapper = mapper;
            this.genreService = genreService;
            this.trackServices = trackServices;
            this.pixabayServices = pixabayServices;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAsync()
        {
            return await db.Playlists.Select(x => mapper.Map<PlaylistDTO>(x)).ToListAsync();
        }

        public async Task<PlaylistDTO> PostAsync(PlaylistDTO obj)
        {
            if (obj.Name.Length < Constants.PLAYLIST_TITLE_MIN_LENGTH)
            {
                throw new Exception(Constants.INVALID_DATA);
            }
            if (await IsExistingAsync(obj.Name))
            {
                throw new Exception(string.Format(Constants.ALREADY_TAKEN, "Title"));
            }

            int currentDuration = 0;
            int tripDurationInSec = (int)obj.Trip.Duration * 60;

            obj.GenresWithPercentages = obj.GenresWithPercentages.Where(x => x.Percentage > 0).ToList();

            //Repeat rtist and Don't use top songs
            if (obj.RepeatArtists && !obj.TopSongs)
            {
                while (!(tripDurationInSec - 300 <= currentDuration && currentDuration <= tripDurationInSec + 300))
                {
                    var currentGenre = await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName);
                    var genreSeconds = tripDurationInSec * obj.GenresWithPercentages.First().Percentage / 100;

                    var tracksReady = trackServices.GetTracks(mapper.Map<Genre>(currentGenre), genreSeconds);

                    foreach (var track in tracksReady)
                    {
                        var sth = new PlaylistTracksDTO
                        {
                            TrackId = track.Id,
                            Track = track
                        };

                        obj.Tracks.Add(sth);
                        currentDuration += track.Duration;
                    }
                    //TODO:This one adds additional genres to the DB
                    //obj.Genres.Add(mapper.Map<PlaylistGenreDTO>(await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName)));
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }
            //Don't repeat rtist and don't use top songs
            else if (!obj.RepeatArtists && !obj.TopSongs)
            {
                while (!(tripDurationInSec - 300 <= currentDuration && currentDuration <= tripDurationInSec + 300))
                {
                    var currentGenre = await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName);
                    var genreSeconds = tripDurationInSec * obj.GenresWithPercentages.First().Percentage / 100;

                    var tracksReady = trackServices.GetTracksWithDistinctArtists(mapper.Map<Genre>(currentGenre), genreSeconds);

                    foreach (var track in tracksReady)
                    {
                        var sth = new PlaylistTracksDTO
                        {
                            TrackId = track.Id,
                            Track = track
                        };

                        obj.Tracks.Add(sth);
                        currentDuration += track.Duration;
                    }

                    var playlistGenreDTO = new PlaylistGenreDTO
                    {
                        GenreId = currentGenre.Id,
                        Name = currentGenre.Name
                    };
                    //TODO:This one adds additional genres to the DB
                    //obj.Genres.Add(playlistGenreDTO);
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }
            //repeat rtist and use top songs
            else if (obj.RepeatArtists && obj.TopSongs)
            {
                while (!(tripDurationInSec - 300 <= currentDuration && currentDuration <= tripDurationInSec + 300))
                {
                    var currentGenre = await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName);
                    var genreSeconds = tripDurationInSec * obj.GenresWithPercentages.First().Percentage / 100;

                    var tracksReady = trackServices.GetTopXTracks(genreSeconds, mapper.Map<Genre>(currentGenre));

                    foreach (var track in tracksReady)
                    {
                        var sth = new PlaylistTracksDTO
                        {
                            TrackId = track.Id,
                            Track = track
                        };

                        obj.Tracks.Add(sth);

                        currentDuration += track.Duration;
                    }

                    var playlistGenreDTO = new PlaylistGenreDTO
                    {
                        GenreId = currentGenre.Id,
                        Name = currentGenre.Name
                    };

                    obj.Genres.Add(playlistGenreDTO);
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }
            //Don't repeat rtist and use top songs
            else
            {
                while (!(tripDurationInSec - 300 <= currentDuration && currentDuration <= tripDurationInSec + 300))
                {
                    var currentGenre = await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName);
                    var genreSeconds = tripDurationInSec * obj.GenresWithPercentages.First().Percentage / 100;

                    var tracksReady = trackServices.GetTopXTracksWithDistinctArtist(genreSeconds, mapper.Map<Genre>(currentGenre));

                    foreach (var track in tracksReady)
                    {
                        var sth = new PlaylistTracksDTO
                        {
                            TrackId = track.Id,
                            Track = track
                        };

                        obj.Tracks.Add(sth);
                        currentDuration += track.Duration;
                    }

                    var playlistGenreDTO = new PlaylistGenreDTO
                    {
                        GenreId = currentGenre.Id,
                        Name = currentGenre.Name
                    };
                    //TODO:This one adds additional genres to the DB
                    //obj.Genres.Add(playlistGenreDTO);
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }

            obj.AvgRank = obj.Tracks.Average(x => x.Track.Rank.Value);

            foreach (var item in obj.Tracks)
            {
                item.Track = null;
            }

            obj.Duration = currentDuration;
            obj.ImagePath = await pixabayServices.GetImageURL();
            var playlist = mapper.Map<Playlist>(obj);
            playlist.CreatedOn = DateTime.Now;

            playlist.TripId = obj.Trip.Id;
            playlist.AudienceId = obj.Audience.Id;
            playlist.Author = null;
            playlist.Trip = null;
            playlist.Audience = null;

            await db.Playlists.AddAsync(playlist);

            await db.SaveChangesAsync();

            return mapper.Map<PlaylistDTO>(obj);
        }

        public async Task<PlaylistDTO> UpdateAsync(int id, UpdatePlaylistDTO obj)
        {
            var playlistToUpdate = await GetPlaylistAsync(id);

            if (String.IsNullOrEmpty(obj.Name))
            {
                throw new Exception(Constants.INVALID_DATA);
            }

            if (obj.Name != playlistToUpdate.Name && obj.Name.Length >= Constants.PLAYLIST_TITLE_MIN_LENGTH)
            {
                if (await IsExistingAsync(obj.Name))
                {
                    throw new Exception(string.Format(Constants.ALREADY_TAKEN, "Title"));
                }
                playlistToUpdate.Name = obj.Name;
            }

            if (obj.Audience.Id != playlistToUpdate.Audience.Id && (obj.Audience.Id >= 1 && obj.Audience.Id <= 3))
            {
                playlistToUpdate.Audience.Id = obj.Audience.Id;
            }

            await db.SaveChangesAsync();

            return mapper.Map<PlaylistDTO>(playlistToUpdate);
        }

        public async Task<PlaylistDTO> DeleteAsync(string title)
        {
            var playlistToDelete = await GetPlaylistAsync(title);

            playlistToDelete.DeletedOn = DateTime.Now;
            playlistToDelete.IsDeleted = true;

            await db.SaveChangesAsync();

            return mapper.Map<PlaylistDTO>(playlistToDelete);
        }

        public async Task<int> PlaylistCount()
        {
            var numOfPlaylists = await GetAsync();
            return numOfPlaylists.Count();
        }

        public async Task<bool> IsExistingAsync(string title)
        {
            return await db.Playlists.AnyAsync(x => x.Name == title);
        }

        private async Task<Playlist> GetPlaylistAsync(int id)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Id == id);
            return playlist ?? throw new InvalidOperationException(Constants.PLAYLIST_NOT_FOUND);
        }

        private async Task<Playlist> GetPlaylistAsync(string title)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Name == title);
            return playlist ?? throw new InvalidOperationException(Constants.PLAYLIST_NOT_FOUND);
        }

        public async Task<PlaylistDTO> GetPlaylistDTOAsync(string title)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Name == title);

            return mapper.Map<PlaylistDTO>(playlist) ?? throw new Exception(Constants.PLAYLIST_NOT_FOUND);
        }

        public async Task<PlaylistDTO> GetPlaylistDTOAsync(int id)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<PlaylistDTO>(playlist) ?? throw new Exception(Constants.PLAYLIST_NOT_FOUND);
        }

        public async Task<Audience> GetAudienceAsync(int id)
        {
            var audience = await db.Audience.FirstOrDefaultAsync(x => x.Id == id);

            return audience ?? throw new Exception(Constants.AUDIENCE_NOT_FOUND);
        }

        public async Task<IEnumerable<PlaylistDTO>> GetUserPlaylists(int userId)
        {

            var playlists = await db.Playlists.Where(x => x.AuthorId == userId).ToListAsync();

            return mapper.Map<IEnumerable<PlaylistDTO>>(playlists);

        }
    }
}