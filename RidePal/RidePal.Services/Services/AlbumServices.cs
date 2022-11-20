using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class AlbumServices : IAlbumService
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public AlbumServices(RidePalContext ridePalContext, IMapper mapper)
        {
            this.db = ridePalContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AlbumDTO>> GetAlbumByArtistAsync(ArtistDTO artist)
        {
            var albums = await db.Albums
                                 .Where(album => album.ArtistId == artist.Id)
                                 .ToListAsync();

            return this.mapper.Map<List<AlbumDTO>>(albums);
        }

        public async Task<IEnumerable<AlbumDTO>> GetAlbumByGenreAsync(GenreDTO genre)
        {
            var albums = await db.Albums
                                .Where(album => album.GenreId == genre.Id)
                                .ToListAsync();

            return this.mapper.Map<List<AlbumDTO>>(albums);
        }

        public async Task<AlbumDTO> GetAlbumByIdAsync(int id)
        {
            var album = await db.Albums.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(string.Format(Constants.ALBUM_NOT_FOUND, id));

            return this.mapper.Map<AlbumDTO>(album);
        }

        public IQueryable<AlbumDTO> GetAlbums()
        {
            return this.mapper.Map<IQueryable<AlbumDTO>>(db.Albums.AsQueryable());
        }
    }
}
