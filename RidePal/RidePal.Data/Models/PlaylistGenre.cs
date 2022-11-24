using MovieForum.Data.Models.Interfaces;
using System;

namespace RidePal.Data.Models
{
    public class PlaylistGenre : IDeletable
    {
        public int? PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public int? GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}