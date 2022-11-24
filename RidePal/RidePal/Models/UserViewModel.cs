using RidePal.Services.DTOModels;
using System.Collections.Generic;

namespace RidePal.WEB.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string ImagePath { get; set; }

        public bool IsBlocked { get; set; }

        public virtual ICollection<PlaylistDTO> Playlists { get; set; } = new List<PlaylistDTO>();

        public virtual ICollection<UserDTO> Friends { get; set; } = new List<UserDTO>();

        public virtual ICollection<FriendRequestDTO> SentFriendRequests { get; set; } = new List<FriendRequestDTO>();
        public virtual ICollection<FriendRequestDTO> ReceivedFriendRequests { get; set; } = new List<FriendRequestDTO>();

        public bool IsDeleted { get; set; }
    }
}