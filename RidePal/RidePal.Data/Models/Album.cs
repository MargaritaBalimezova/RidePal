using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Album : IHasId, IDeletable
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int? ArtistId { get; set; }

        public virtual Artist Artist { get; set; }

        [Required]
        public int? GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
