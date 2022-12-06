using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace RidePal.Data.Models
{
    public class Playlist : IHasId, IDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int Duration { get; set; }

        public double AvgRank { get; set; }

        public int? AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int? TripId { get; set; }

        public virtual Trip Trip { get; set; }

        public int? AudienceId { get; set; }

        public virtual Audience Audience { get; set; }

        public bool RepeatArtists { get; set; }
        public bool TopSongs { get; set; }

        public virtual ICollection<PlaylistTracks> Tracks { get; set; } = new List<PlaylistTracks>();

        public virtual ICollection<PlaylistGenre> Genres { get; set; } = new List<PlaylistGenre>();

        public int LikesCount { get; set; }

        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}