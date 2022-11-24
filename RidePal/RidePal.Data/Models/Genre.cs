using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Data.Models
{
    public class Genre : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<PlaylistGenre> Playlists { get; set; } = new List<PlaylistGenre>();

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}