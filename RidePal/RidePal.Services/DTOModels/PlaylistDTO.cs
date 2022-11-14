using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class PlaylistDTO
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
        public virtual User Author { get; set; }

        [Required]
        public virtual Trip Trip { get; set; }

        [Required]
        public virtual Audience Audience { get; set; }

        public bool RepeatArtists { get; set; }

        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

        public virtual IDictionary<Genre, int> GenresWithPercentages { get; set; } = new Dictionary<Genre, int>();

        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}
