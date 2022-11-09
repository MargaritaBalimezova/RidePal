using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Models
{
    public class Track: IHasId, IDeletable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rank { get; set; }
        public int Duration { get; set; }
        public Album Album { get; set; }
        public Artist Artist { get; set; }
        public Genre Genre { get; set; }
        public string PreviewURL { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
