using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class PlaylistServices : IPlaylistServices
    {
        private const string PLAYLIST_NOT_FOUND = "Playlist not found!";
        private const string GENRE_NOT_FOUND = "Genre not found!";
        private const string INVALID_DATA = "Invalid Data!";

        private readonly RidePalContext db;
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly ITrackServices trackServices;

        public PlaylistServices(RidePalContext context, IMapper mapper, IGenreService genreService, ITrackServices trackServices)
        {
            this.db = context;
            this.mapper = mapper;
            this.genreService = genreService;
            this.trackServices = trackServices;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAsync()
        {
            return await db.Users.Select(x => mapper.Map<PlaylistDTO>(x)).ToListAsync();
        }

        public async Task<PlaylistDTO> PostAsync(PlaylistDTO obj)
        {
            //https://pixabay.com/api/?key=31208625-54e60a24a8bb2ce33717762bd&q=yellow+flowers&image_type=photo

            if (String.IsNullOrEmpty(obj.Name))
            {
                throw new Exception(INVALID_DATA);
            }
            if (await IsExistingAsync(obj.Name))
            {
                throw new Exception("Title already taken");
            }

            int currentDuration = 0;
            int tripDurationInSec = (int)obj.Trip.Duration * 60;

            //TODO optimize
            obj.GenresWithPercentages = obj.GenresWithPercentages.Where(x => x.Percentage > 0).ToList();

            if (obj.RepeatArtists)
            {
                while (!(tripDurationInSec - 300 <= currentDuration && currentDuration <= tripDurationInSec + 300))
                {
                    var currentGenre = await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName);
                    var genreSeconds = tripDurationInSec * obj.GenresWithPercentages.First().Percentage / 100;

                    var tracksReady = trackServices.GetTracks(mapper.Map<Genre>(currentGenre), genreSeconds);

                    foreach (var track in tracksReady)
                    {
                        obj.Tracks.Add(mapper.Map<TrackDTO>(track));
                        currentDuration += track.Duration;
                    }

                    obj.Genres.Add(mapper.Map<PlaylistGenreDTO>(await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName)));
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }
            else
            {
                while (!(tripDurationInSec - 300 <= currentDuration && currentDuration <= tripDurationInSec + 300))
                {
                    var currentGenre = await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName);
                    var genreSeconds = tripDurationInSec * obj.GenresWithPercentages.First().Percentage / 100;

                    var tracksReady = trackServices.GetTracksWithDistinctArtists(mapper.Map<Genre>(currentGenre), genreSeconds);

                    foreach (var track in tracksReady)
                    {
                        obj.Tracks.Add(mapper.Map<TrackDTO>(track));
                        currentDuration += track.Duration;
                    }

                    obj.Genres.Add(mapper.Map<PlaylistGenreDTO>(await genreService.GetGenreByName(obj.GenresWithPercentages.First().GenreName)));
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }
            //TODO top songs

            obj.AvgRank = obj.Tracks.Average(x => x.Rank.Value);
            obj.Duration = currentDuration;

            var playlist = mapper.Map<Playlist>(obj);

            playlist.Trip = null;
            playlist.TripId = null;

            await db.Playlists.AddAsync(playlist);
            await db.SaveChangesAsync();

            return mapper.Map<PlaylistDTO>(obj);
        }

        public async Task<PlaylistDTO> UpdateAsync(int id, UpdatePlaylistDTO obj)
        {
            var playlistToUpdate = await GetPlaylistAsync(id);

            if (!String.IsNullOrEmpty(obj.Name))
            {
                throw new Exception(INVALID_DATA);
            }

            if (obj.Name != playlistToUpdate.Name)
            {
                if (await IsExistingAsync(obj.Name))
                {
                    throw new Exception("Title already taken");
                }
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
            return playlist ?? throw new InvalidOperationException(PLAYLIST_NOT_FOUND);
        }

        private async Task<Playlist> GetPlaylistAsync(string title)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Name == title);
            return playlist ?? throw new InvalidOperationException(PLAYLIST_NOT_FOUND);
        }

        public async Task<PlaylistDTO> GetPlaylistDTOAsync(string title)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Name == title);

            return mapper.Map<PlaylistDTO>(playlist) ?? throw new Exception(PLAYLIST_NOT_FOUND);
        }

        public async Task<PlaylistDTO> GetPlaylistDTOAsync(int id)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<PlaylistDTO>(playlist) ?? throw new Exception(PLAYLIST_NOT_FOUND);
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            var genres = await db.Genres.ToListAsync();
            return genres;
        }
    }
}