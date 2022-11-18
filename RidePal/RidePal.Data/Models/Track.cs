using MovieForum.Data.Models.Interfaces;
using RidePal.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Track : IDeletable, IHasLongId
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Rank { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public long? AlbumId { get; set; }        
        public virtual Album Album { get; set; }
        [Required]
        public long ArtistId { get; set; }        
        public virtual Artist Artist { get; set; }
        [Required]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        [Required]
        public string PreviewURL { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var other = (Track)obj;

            return this.Id == other.Id && this.Title == other.Title;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
