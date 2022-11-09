using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Models
{
    internal class Trip : IHasId, IDeletable
    {
        public int Id { get; set; }
        public string StartPoint { get; set; }
        public string Destination { get; set; }
        public int Duration { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
