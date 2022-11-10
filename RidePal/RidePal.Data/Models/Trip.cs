using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Trip : IHasId, IDeletable
    {
        public int Id { get; set; }
        [Required]
        public string StartPoint { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int PlaylistId { get; set; }
        [Required]
        public virtual Playlist Playlist { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
