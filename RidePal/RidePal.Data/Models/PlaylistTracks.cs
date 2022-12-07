using RidePal.Data.Models.Interfaces;
using System;

namespace RidePal.Data.Models
{
    public class PlaylistTracks : IDeletable
    {
        public int? PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public long? TrackId { get; set; }
        public virtual Track Track { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}