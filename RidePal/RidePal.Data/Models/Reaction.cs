using RidePal.Data.Models.Interfaces;
using System;

namespace RidePal.Data.Models
{
    public class Reaction : IHasId, IDeletable
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool Liked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}