using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    class Playlist : IHasId, IDeletable
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
        //TODO Trip
        public string Trip { get; set; }

        [Required]
        public int? AudienceId { get; set; }

        [Required]
        public Audience Audience { get; set; }

        [Required]
        public ICollection<Track> Tracks { get; set; } = new List<Track>();

        [Required]
        public ICollection<Genre> Grenres { get; set; } = new List<Genre>();

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
