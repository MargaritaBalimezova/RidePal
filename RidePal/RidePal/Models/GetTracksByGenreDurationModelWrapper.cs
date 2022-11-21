using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Models
{
    public class GetTracksByGenreDurationModelWrapper
    {
        public Genre Genre { get; set; }
        public int Duration { get; set; }
    }
}
