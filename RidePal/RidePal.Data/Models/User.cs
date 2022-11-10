using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class User : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress()]
        public string Email { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

        public virtual ICollection<User> Friends { get; set; } = new List<User>();

        public virtual ICollection<FriendRequest> SentFriendRequests { get; set; } = new List<FriendRequest>();
        public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
