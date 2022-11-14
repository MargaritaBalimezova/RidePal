using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class PlaylistServices
    {
        private const string PLAYLIST_NOT_FOUND = "Playlist not found!";
        private const string GENRE_NOT_FOUND = "Genre not found!";
        private const string INVALID_DATA = "Invalid Data!";

        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public PlaylistServices(RidePalContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAsync()
        {
            return await db.Users.Select(x => mapper.Map<PlaylistDTO>(x)).ToListAsync();
        }

        public async Task<PlaylistDTO> PostAsync(PlaylistDTO obj)
        {

            //https://pixabay.com/api/?key=31208625-54e60a24a8bb2ce33717762bd&q=yellow+flowers&image_type=photo

            //    obj.ImagePath = 
            HashSet<Artist> artists = new HashSet<Artist>();

            if (String.IsNullOrEmpty(obj.Name))
            {
                throw new Exception(INVALID_DATA);
            }

            int numOfGenres = obj.GenresWithPercentages.Count();

            if (obj.RepeatArtists)
            {
                double currentDuration = 0;
                while (!(obj.Trip.Duration - 5 <= currentDuration && currentDuration <= obj.Trip.Duration + 5))
                {
                    

                    //obj.Tracks.Add(track);
                    //currentDuration += track.Duration;
                }
            }
            else
            {
                double currentDuration = 0;
                while (!(obj.Trip.Duration - 5 <= currentDuration && currentDuration <= obj.Trip.Duration + 5))
                {
                    Track track = new Track();

                    artists.Add(track.Artist);

                    obj.Tracks.Add(track);
                    currentDuration += track.Duration;
                }
            }

            obj.Duration = obj.Tracks.Sum(x => x.Duration);

            obj.AvgRank = obj.Tracks.Sum(x => x.Rank) / obj.Tracks.Count();

            var playlist = mapper.Map<Playlist>(obj);


            await db.Playlists.AddAsync(playlist);
            await db.SaveChangesAsync();

            return mapper.Map<PlaylistDTO>(obj);
        }

        public async Task<PlaylistDTO> UpdateAsync(int id, UpdatePlaylistDTO obj)
        {
            var playlistToUpdate = await GetPlaylistAsync(id);

            if (!String.IsNullOrEmpty(obj.Name))
            {
                playlistToUpdate.Name = obj.Name;
            }
            if (obj.Audience.Id != playlistToUpdate.Audience.Id && (obj.Audience.Id >= 1 && obj.Audience.Id <= 3))
            {
                playlistToUpdate.Audience.Id = obj.Audience.Id;
            }
            if (obj.Audience.Id != playlistToUpdate.Audience.Id && (obj.Audience.Id >= 1 && obj.Audience.Id <= 3))
            {
                playlistToUpdate.Audience.Id = obj.Audience.Id;
            }

            foreach (var genre in obj.Genres)
            {
                Genre genre1 = await db.Genres.FirstOrDefaultAsync(x => x.Id == genre.Id)
                     ?? throw new InvalidOperationException(GENRE_NOT_FOUND);
                //playlistToUpdate.Genres.Add(genre1);
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
    }
}
