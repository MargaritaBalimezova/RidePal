using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IPlaylistServices
    {
        Task<IEnumerable<PlaylistDTO>> GetAsync();

        Task<PlaylistDTO> PostAsync(PlaylistDTO obj);

        Task<PlaylistDTO> UpdateAsync(int id, UpdatePlaylistDTO obj);

        Task<PlaylistDTO> DeleteAsync(string title);

        Task<int> PlaylistCount();

        Task<bool> IsExistingAsync(string title);

        Task<PlaylistDTO> GetPlaylistDTOAsync(string title);

        Task<PlaylistDTO> GetPlaylistDTOAsync(int id);

        Task<Audience> GetAudienceAsync(int id);

        Task<IEnumerable<PlaylistDTO>> GetUserPlaylists(int userId);

        Task<PaginatedList<PlaylistDTO>> FilterPlaylists(PlaylistQueryParameters parameters);

        Task Like(UserDTO user, int playListId);
    }
}