using RidePal.Services.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IArtistService
    {
        Task<string> GetArtistStyle(int id);
        Task<IEnumerable<TrackDTO>> GetArtistTopTracks(int id);
        Task<IEnumerable<ArtistDTO>> GetTopArtists(int x);
        Task<IEnumerable<ArtistDTO>> GetArtistsAsync();

        Task<ArtistDTO> GetArtistByIdAsync(int id);

        Task<ArtistDTO> GetArtistByNameAsync(string name);

        Task<IEnumerable<AlbumDTO>> GetArtistAlbumsByArtistAsync(int id);

        Task<IEnumerable<TrackDTO>> GetArtistTracks(int id);
    }
}