using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<AlbumDTO>> GetArtistAlbumsByArtistAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<List<AlbumDTO>>(artist.Albums);
        }

        public async Task<ArtistDTO> GetArtistByIdAsync(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<ArtistDTO>(artist);
        }

        public async Task<IEnumerable<AlbumDTO>> GetArtistsAsync()
        {
            var artists = await db.Artists.ToListAsync();

            return this.mapper.Map<List<AlbumDTO>>(artists);
        }

        public async Task<IEnumerable<TrackDTO>> GetArtistTracks(int id)
        {
            var artist = await db.Artists.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            return this.mapper.Map<List<TrackDTO>>(artist.Tracks);
        }
    }
}
