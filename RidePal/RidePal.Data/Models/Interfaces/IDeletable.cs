using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models.Interfaces
{
    public interface IDeletable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
