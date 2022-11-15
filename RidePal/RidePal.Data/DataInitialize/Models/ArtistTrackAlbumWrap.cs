using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.DataInitialize.Models
{
    public class ArtistTrackAlbumWrap
    {
        public List<Artist> artists { get; set; } = new List<Artist>();
        public List<Track> tracks  { get; set;} = new List<Track>();
        public List<Album> albums {get; set;} = new List<Album>();
    }
}
