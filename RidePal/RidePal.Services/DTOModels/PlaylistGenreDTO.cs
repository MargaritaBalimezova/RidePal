using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Models
{
    public class PlaylistGenreDTO
    {
        public int? GenreId { get; set; }
        public string Name { get; set; }
        public int? PlaylistId { get; set; }
    }
}