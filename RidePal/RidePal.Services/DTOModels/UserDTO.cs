using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string ImagePath { get; set; }

        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public bool IsGoogleAccount { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public int NumOfBlocks { get; set; }
        public DateTime LastBlockTime { get; set; }

        public ICollection<PlaylistDTO> Playlists { get; set; }
        public ICollection<UserDTO> Friends { get; set; }
        public ICollection<FriendRequestDTO> SentFriendRequests { get; set; }
        public ICollection<FriendRequestDTO> ReceivedFriendRequests { get; set; }
    }
}