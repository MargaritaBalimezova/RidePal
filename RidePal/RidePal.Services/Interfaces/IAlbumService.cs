using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IAlbumService
    {
        IQueryable<AlbumDTO> GetAlbums();
        Task<AlbumDTO> GetAlbumByIdAsync(int id);
        Task<IEnumerable<AlbumDTO>> GetAlbumByArtistAsync(ArtistDTO artist);
        Task<IEnumerable<AlbumDTO>> GetAlbumByGenreAsync(GenreDTO genre);
    }
}
