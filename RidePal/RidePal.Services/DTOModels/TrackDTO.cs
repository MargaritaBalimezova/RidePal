using RidePal.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class TrackDTO
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int? Rank { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public long? AlbumId { get; set; }

        public AlbumDTO Album { get; set; }

        [Required]
        public long ArtistId { get; set; }

        public ArtistDTO Artist { get; set; }

        [Required]
        public int GenreId { get; set; }

        public GenreDTO Genre { get; set; }

        [Required]
        public string PreviewURL { get; set; }

        [Required]
        public string ImagePath { get; set; }
    }
}