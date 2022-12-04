using RidePal.Services.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IArtistService
    {
        Task<string> GetArtistStyleAsync(int id);
        Task<IEnumerable<TrackDTO>> GetArtistTopTracksAsync(int id);
        Task<IEnumerable<ArtistDTO>> GetTopArtists(int x);
        Task<IEnumerable<ArtistDTO>> GetArtistsAsync();

        Task<ArtistDTO> GetArtistByIdAsync(int id);

        Task<ArtistDTO> GetArtistByNameAsync(string name);

        Task<IEnumerable<AlbumDTO>> GetArtistAlbumsByArtistAsync(int id);

        Task<IEnumerable<TrackDTO>> GetArtistTracksAsync(int id);
    }
}