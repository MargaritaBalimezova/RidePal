using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Playlist : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int AvgRank { get; set; }

        [Required]
        public int? AuthorId { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        public int? TripId { get; set; }
        [Required]
        public virtual Trip Trip { get; set; }       

        [Required]
        public int? AudienceId { get; set; }

        [Required]
        public virtual Audience Audience { get; set; }

        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
        
        public virtual ICollection<Genre> Grenres { get; set; } = new List<Genre>();

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
