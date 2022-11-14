using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IUserServices 
    {
        public Task<IEnumerable<UserDTO>> GetAsync();
        public Task<UserDTO> PostAsync(UpdateUserDTO obj);
        public Task<UserDTO> UpdateAsync(int id, UpdateUserDTO obj);
        public Task<UserDTO> DeleteAsync(string email);

        public Task<int> UserCount();
        public Task<bool> IsExistingAsync(string email);
        public Task<bool> IsExistingUsernameAsync(string username);

        public Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        public Task<UserDTO> GetUserDTOAsync(string username);
        public Task<UserDTO> GetUserDTOAsync(int id);

        public Task SendFriendRequest(string sender, string recipient);
        public Task AcceptFriendRequest(string sender, string recipient);
        public Task DeclineFriendRequest(string sender, string recipient);
        public Task RemoveFriend(string username, string friendUsername);

        public Task BlockUser(int id);
        public Task UnblockUser(int id);

        public Task<IEnumerable<UserDTO>> Search(string userSearch, int type);

        public Task<IEnumerable<UserDTO>> GetAllCommentsAsync(string username);
        public Task<IEnumerable<Playlist>> GetAllPlaylistsAsync(string username);


    }
}
