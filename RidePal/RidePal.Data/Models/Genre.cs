using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Genre : IHasId, IDeletable
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

       // public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
