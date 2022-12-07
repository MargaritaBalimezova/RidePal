using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class SearchServices : ISearchService
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public SearchServices(RidePalContext ridePalContext, IMapper mapper)
        {
            this.db = ridePalContext;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<AlbumDTO>> SearchAlbumssAsync(string name)
        {
            name = name.ToLower();
            var albums = await db.Albums
                .Where(album => album.Name.ToLower().Contains(name))
                .OrderByDescending(x => x.Name.ToLower().StartsWith(name) ? 1 : 0)
                .Take(12)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<AlbumDTO>>(albums);
        }

        public async Task<IEnumerable<ArtistDTO>> SearchArtistsAsync(string name)
        {
            name = name.ToLower();
            var artists = await db.Artists
                .Where(artist => artist.Name.ToLower().Contains(name))
                .OrderByDescending(x => x.Name.ToLower().StartsWith(name) ? 1 : 0)
                .Take(12)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<ArtistDTO>>(artists);
        }

        public async Task<IEnumerable<TrackDTO>> SearchTracksAsync(string name)
        {
            name = name.ToLower();
            var tracks = await db.Tracks
                .Where(track => track.Title.ToLower().Contains(name) || track.Artist.Name.ToLower().Contains(name))
                .OrderByDescending(x => x.Title.ToLower().StartsWith(name) ? 1 : 0)
                .Take(50)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<TrackDTO>>(tracks);
        }

        public async Task<IEnumerable<UserDTO>> SearchUsersAsync(string name)
        {
            name = name.ToLower();
            var users = await db.Users
                 .Where(user => user.Username.ToLower().Contains(name))
                 .OrderByDescending(x => x.Username.ToLower().StartsWith(name) ? 1 : 0)
                 .Take(12)
                 .ToListAsync();

            return this.mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<IEnumerable<PlaylistDTO>> SearchPlaylistsAsync(string name)
        {
            name = name.ToLower();
            var playlists = await db.Playlists
                .Where(playlist => playlist.Name.ToLower().Contains(name) && playlist.Audience.Name.ToLower() == "public")
                .OrderByDescending(x => x.Name.ToLower().StartsWith(name) ? 1 : 0)
                .Take(12)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<PlaylistDTO>>(playlists);
        }
    }
}
