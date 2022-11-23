using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistDTO>> GetArtistsAsync();
        Task<ArtistDTO> GetArtistByIdAsync(int id);
        Task<ArtistDTO> GetArtistByNameAsync(string name);
        Task<IEnumerable<AlbumDTO>> GetArtistAlbumsByArtistAsync(int id);
        Task<IEnumerable<TrackDTO>> GetArtistTracks(int id);
    }
}
