using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class AlbumDTO
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long ArtistId { get; set; }

        [Required]
        public int? GenreId { get; set; }

        public GenreDTO Genre { get; set; }

        public ICollection<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
    }
}