using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.DataInitialize.Models
{
    public class PlaylistsResult
    {
        public TrackResult[] data { get; set; }
        public int total { get; set; }
        public string next { get; set; }
    }

    public class TrackResult
    {
        public long id { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public bool _public { get; set; }
        public bool is_loved_track { get; set; }
        public bool collaborative { get; set; }
        public int nb_tracks { get; set; }
        public int fans { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string picture_small { get; set; }
        public string picture_medium { get; set; }
        public string picture_big { get; set; }
        public string picture_xl { get; set; }
        public string checksum { get; set; }
        public string tracklist { get; set; }
        public string creation_date { get; set; }
        public string md5_image { get; set; }
        public string picture_type { get; set; }
        public int time_add { get; set; }
        public int time_mod { get; set; }
        public string type { get; set; }
    }

}
