using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.WEB.Models
{
    public class CreatePlaylistViewModel
    {
        [Required]
        [MinLength(Constants.PLAYLIST_TITLE_MIN_LENGTH, ErrorMessage = "Title length should be more than 4 characters!")]
        public string Name { get; set; }

        public virtual TripDTO Trip { get; set; } = new TripDTO();

        [Required]
        public int AudienceId { get; set; }

        public bool RepeatArtists { get; set; }
        public bool TopSongs { get; set; }
        public virtual List<GenreWithPercentage> GenresWithPercentages { get; set; } = new List<GenreWithPercentage>();
        public DateTime? CreatedOn { get; set; }
    }
}