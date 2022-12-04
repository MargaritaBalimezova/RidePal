using RidePal.Data.Models;
using RidePal.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RidePal.WEB.Models
{
    public class UpdatePlaylistViewModel
    {
        public int Id { get; set; }

        [MinLength(Constants.PLAYLIST_TITLE_MIN_LENGTH, ErrorMessage = "Title length should be more than 4 characters!")]
        public string Name { get; set; }

        public string ImagePath { get; set; }
        public int AudienceId { get; set; }
    }
}