using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<AlbumDTO>> GetArtistsAsync();
        Task<ArtistDTO> GetArtistByIdAsync(int id);
        Task<IEnumerable<AlbumDTO>> GetArtistAlbumsByArtistAsync(int id);
        Task<IEnumerable<TrackDTO>> GetArtistTracks(int id);
    }
}
