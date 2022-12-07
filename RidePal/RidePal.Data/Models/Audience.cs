using RidePal.Data.Models.Interfaces;
using System;

namespace RidePal.Data.Models
{
    public class Audience : IHasId, IDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}