using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Track: IHasId, IDeletable
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Rank { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int AlbumId { get; set; }        
        public virtual Album Album { get; set; }
        [Required]
        public int ArtistId { get; set; }        
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
    }
}
