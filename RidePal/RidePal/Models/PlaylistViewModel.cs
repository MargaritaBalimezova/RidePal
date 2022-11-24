using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;

namespace RidePal.WEB.Models
{
    public class PlaylistViewModel
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
        public virtual ICollection<PlaylistTracksDTO> Tracks { get; set; } = new List<PlaylistTracksDTO>();

        public virtual ICollection<PlaylistGenreDTO> Genres { get; set; } = new List<PlaylistGenreDTO>();
    }
}