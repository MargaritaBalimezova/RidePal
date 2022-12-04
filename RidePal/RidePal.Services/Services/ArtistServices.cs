using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class ArtistServices : IArtistService
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public ArtistServices(RidePalContext ridePalContext, IMapper mapper)
        {
            this.db = ridePalContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDTO>> GetTopArtists(int x)
        {
            var artists = await db.Artists
                            .OrderByDescending(x => x.Tracks.Max(x => x.Rank))
                            .Take(x)
                            .ToListAsync();

            return this.mapper.Map<IEnumerable<ArtistDTO>>(artists);
        }

        public async Task<IEnumerable<AlbumDTO>> GetArtistAlbumsByArtistAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<IEnumerable<AlbumDTO>>(artist.Albums);
        }

        public async Task<ArtistDTO> GetArtistByIdAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<ArtistDTO>(artist);
        }

        public async Task<ArtistDTO> GetArtistByNameAsync(string name)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Name == name)
                           ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<ArtistDTO>(artist);
        }

        public async Task<IEnumerable<ArtistDTO>> GetArtistsAsync()
        {
            var artists = await db.Artists.ToListAsync();

            return this.mapper.Map<IEnumerable<ArtistDTO>>(artists);
        }

        public async Task<IEnumerable<TrackDTO>> GetArtistTracksAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<IEnumerable<TrackDTO>>(artist.Tracks);
        }

        public async Task<string> GetArtistStyleAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            var genre = artist.Tracks.GroupBy(i => i.Genre.Name).OrderByDescending(grp => grp.Count())
                    .Select(grp => grp.Key).First();

            return genre;
        }

        public async Task<IEnumerable<TrackDTO>> GetArtistTopTracksAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<IEnumerable<TrackDTO>>(artist.Tracks.OrderByDescending(x => x.Rank).Take(10));
        }
    }
}