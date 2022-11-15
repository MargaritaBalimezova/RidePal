﻿using MovieForum.Data.Models.Interfaces;
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

        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

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

            var other = (Artist)obj;

            return this.Id == other.Id && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
