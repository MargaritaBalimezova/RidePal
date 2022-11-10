using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }

        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<UserDTO> Friends { get; set; }
        public ICollection<FriendRequestDTO> SentFriendRequests { get; set; }
        public ICollection<FriendRequestDTO> ReceivedFriendRequests { get; set; }
    }
}
