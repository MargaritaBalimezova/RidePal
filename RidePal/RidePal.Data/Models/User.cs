using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace RidePal.Data.Models
{
    public class User : IHasId, IDeletable
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string ImagePath { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public bool IsBlocked { get; set; }

        public int NumOfBlocks { get; set; }

        public DateTime LastBlockTime { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsGoogleAccount { get; set; }
        public string AccessToken { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

        public virtual ICollection<User> Friends { get; set; } = new List<User>();

        public virtual ICollection<FriendRequest> SentFriendRequests { get; set; } = new List<FriendRequest>();
        public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}