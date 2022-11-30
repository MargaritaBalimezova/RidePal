using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Models
{
    public class GenreTopTracksWrapModel
    {
        public GenreDTO Genre { get; set; }
        public IEnumerable<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
    }
}
