using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Album : IHasId, IDeletable
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int? ArtistId { get; set; }

        public virtual Artist Artist { get; set; }

        [Required]
        public int? GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            
            var other = (Album)obj;

            return this.Id == other.Id && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
