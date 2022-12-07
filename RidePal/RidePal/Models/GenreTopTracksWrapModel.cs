using RidePal.Services.DTOModels;
using System.Collections.Generic;

namespace RidePal.WEB.Models
{
    public class GenreTopTracksWrapModel
    {
        public GenreDTO Genre { get; set; }
        public IEnumerable<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
    }
}
