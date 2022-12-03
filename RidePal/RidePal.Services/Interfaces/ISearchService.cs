using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<TrackDTO>> SearchTracksAsync(string name);
        Task<IEnumerable<UserDTO>> SearchUsersAsync(string name);
        Task<IEnumerable<AlbumDTO>> SearchAlbumssAsync(string name);
        Task<IEnumerable<ArtistDTO>> SearchArtistsAsync(string name);
    }
}
