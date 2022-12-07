using System;

namespace RidePal.Data.Models.Interfaces
{
    public interface IDeletable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}