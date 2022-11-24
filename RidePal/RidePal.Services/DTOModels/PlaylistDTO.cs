using RidePal.Data.Models;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;

namespace RidePal.Services.DTOModels
{
    public class PlaylistDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int Duration { get; set; }

        public double AvgRank { get; set; }

        public virtual UserDTO Author { get; set; }

        public virtual TripDTO Trip { get; set; }

        public virtual Audience Audience { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool RepeatArtists { get; set; }
        public bool TopSongs { get; set; }

        public virtual ICollection<PlaylistTracksDTO> Tracks { get; set; } = new List<PlaylistTracksDTO>();

        public virtual List<GenreWithPercentage> GenresWithPercentages { get; set; } = new List<GenreWithPercentage>();

        public virtual ICollection<PlaylistGenreDTO> Genres { get; set; } = new List<PlaylistGenreDTO>();
    }
}