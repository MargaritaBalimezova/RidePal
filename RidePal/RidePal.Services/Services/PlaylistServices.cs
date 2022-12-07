using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.AspNetCore.Http;

namespace RidePal.Services.Services
{
    public class PlaylistServices : IPlaylistServices
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly ITrackServices trackServices;
        private readonly IPixabayServices pixabayServices;
        private readonly IAWSCloudStorageService storageService;

        public PlaylistServices(RidePalContext context, IMapper mapper, 
            IGenreService genreService, ITrackServices trackServices, 
            IPixabayServices pixabayServices, IAWSCloudStorageService storageService)
        {
            this.db = context;
            this.mapper = mapper;
            this.genreService = genreService;
            this.trackServices = trackServices;
            this.pixabayServices = pixabayServices;
            this.storageService = storageService;
        }

        #region CRUD

        public async Task<IEnumerable<PlaylistDTO>> GetAsync()
        {
            return await db.Playlists.Select(x => mapper.Map<PlaylistDTO>(x)).ToListAsync();
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

            if (obj.Audience.Id != playlistToUpdate.AudienceId && (obj.Audience.Id >= 1 && obj.Audience.Id <= 3))
            {
                playlistToUpdate.AudienceId = obj.Audience.Id;
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

                    var playlistGenreDTO = new PlaylistGenreDTO
                    {
                        GenreId = currentGenre.Id,
                        Name = currentGenre.Name
                    };

                    obj.Genres.Add(playlistGenreDTO);
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

                    obj.Genres.Add(playlistGenreDTO);
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

                    obj.Genres.Add(playlistGenreDTO);
                    obj.GenresWithPercentages.Remove(obj.GenresWithPercentages.First());
                }
            }

            obj.AvgRank = obj.Tracks.Average(x => x.Track.Rank.Value);

            foreach (var item in obj.Tracks)
            {
                item.Track = null;
            }

            obj.Duration = currentDuration;

            var imageUrl = await pixabayServices.GetImageURL();
            var imagePath = DownloadAndSavePhoto(imageUrl, obj.Name);
            obj.ImagePath = await storageService.UploadPlaylistImage(imagePath);

            var playlist = mapper.Map<Playlist>(obj);
            playlist.CreatedOn = DateTime.Now;

            playlist.TripId = obj.Trip.Id;
            playlist.AudienceId = obj.Audience.Id;
            playlist.Author = null;
            playlist.Trip = null;
            playlist.Audience = null;

            foreach (var item in playlist.Genres)
            {
                item.Genre = null;
            }

            await db.Playlists.AddAsync(playlist);

            await db.SaveChangesAsync();

            return mapper.Map<PlaylistDTO>(obj);
        }

        #endregion CRUD

        #region Get Playlist

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

        public async Task<IEnumerable<PlaylistDTO>> GetUserPlaylists(int userId)
        {
            var playlists = await db.Playlists.Where(x => x.AuthorId == userId).ToListAsync();

            return mapper.Map<IEnumerable<PlaylistDTO>>(playlists);
        }

        #endregion Get Playlist

        public async Task<int> PlaylistCount()
        {
            var numOfPlaylists = await GetAsync();
            return numOfPlaylists.Count();
        }

        public async Task<bool> IsExistingAsync(string title)
        {
            return await db.Playlists.AnyAsync(x => x.Name == title);
        }

        public async Task<Audience> GetAudienceAsync(int id)
        {
            var audience = await db.Audience.FirstOrDefaultAsync(x => x.Id == id);

            return audience ?? throw new Exception(Constants.AUDIENCE_NOT_FOUND);
        }

        public async Task<PaginatedList<PlaylistDTO>> FilterPlaylists(PlaylistQueryParameters parameters)
        {
            var result = db.Playlists
                .Where(x => x.Audience.Name.ToLower() == "public")
                .AsQueryable();

            if (parameters.Duration.HasValue && parameters.Duration != 0)
            {
                parameters.Duration = parameters.Duration * 3600;
                if (parameters.Duration != 5 * 3600)
                {
                    result = result
                            .Where(x => x.Duration <= parameters.Duration);
                }
                else
                {
                    result = result
                            .Where(x => x.Duration >= parameters.Duration);
                }
            }

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("name", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.Name);
                }
                else if (parameters.SortBy.Equals("duration", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.Duration);
                }
                else if(parameters.SortBy.Equals("rating", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.LikesCount);
                }

                if (!string.IsNullOrEmpty(parameters.SortOrder)
                    && parameters.SortOrder.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.Reverse();
                }
            }

            int totalPages = (result.Count() + 1) / parameters.PageSize;
            if (result.Count() % parameters.PageSize != 0)
            {
                totalPages++;
            }

            var resultList = Paginate(await result.ToListAsync(), parameters.PageNumber, parameters.PageSize);

            return new PaginatedList<PlaylistDTO>(resultList.ToList(), totalPages, parameters.PageNumber);
        }

        public async Task Like(UserDTO user, int playListId)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(x => x.Id == playListId) ?? throw new Exception(Constants.PLAYLIST_NOT_FOUND);

            var reaction = playlist.Reactions.FirstOrDefault(x => x.UserId == user.Id);

            if (reaction == null)
            {
                reaction = new Reaction
                {
                    UserId = user.Id,
                    Liked = true
                };

                playlist.Reactions.Add(reaction);
                playlist.LikesCount++;

                await db.Reactions.AddAsync(reaction);
                await db.SaveChangesAsync();

            }
            else if (reaction != null && reaction.Liked == true)
            {
                reaction.Liked = false;
                playlist.LikesCount--;
                await db.SaveChangesAsync();
            }
            else if (reaction != null && reaction.Liked == false)
            {
                reaction.Liked = true;
                playlist.LikesCount++;
                await db.SaveChangesAsync();
            }             

              

        }


        #region Private Methods

        private string DownloadAndSavePhoto(string url, string playlistName)
        {
            string directory = "D:\\Downloads\\RidePalImages";
            string filePath = directory + "\\" + playlistName + ".jpg";
            using (WebClient client = new WebClient())
            {
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
                client.DownloadFile(new Uri(url), filePath);
            }
            return filePath;
        }

        private IEnumerable<PlaylistDTO> Paginate(IEnumerable<Playlist> playlists, int pageNumber, int pageSize)
        {
            return this.mapper.Map<IEnumerable<PlaylistDTO>>(playlists.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        #endregion Private Methods
    }

}