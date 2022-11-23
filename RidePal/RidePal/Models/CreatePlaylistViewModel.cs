using RidePal.Data.Models;
using RidePal.Services.Models;
using System.Collections.Generic;

namespace RidePal.WEB.Models
{
    public class CreatePlaylistViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int Duration { get; set; }

        public double AvgRank { get; set; }

        public virtual User Author { get; set; }

        public virtual Trip Trip { get; set; }

        public virtual Audience Audience { get; set; }

        public bool RepeatArtists { get; set; }
        public bool TopSongs { get; set; }

        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

        public virtual List<GenreWithPercentage> GenresWithPercentages { get; set; } = new List<GenreWithPercentage>();

        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}