using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.WEB.Models
{
    public class PlaylistViewModel
    {
        public int Id { get; set; }
        [MinLength(Constants.PLAYLIST_TITLE_MIN_LENGTH, ErrorMessage = "Title length should be more than 4 characters!")]
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int Duration { get; set; }

        public double AvgRank { get; set; }

        public int LikesCount { get; set; }

        public virtual UserDTO Author { get; set; }

        public virtual TripDTO Trip { get; set; }

        public virtual Audience Audience { get; set; }
        public DateTime? CreatedOn { get; set; }
        public virtual ICollection<PlaylistTracksDTO> Tracks { get; set; } = new List<PlaylistTracksDTO>();

        public virtual ICollection<PlaylistGenreDTO> Genres { get; set; } = new List<PlaylistGenreDTO>();
    }
}