using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Models
{
    public class Audience : IHasId
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
