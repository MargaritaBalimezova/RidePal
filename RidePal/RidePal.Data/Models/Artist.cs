using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Artist : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        
        public DateTime? DeletedOn { get; set; }
    }
}
