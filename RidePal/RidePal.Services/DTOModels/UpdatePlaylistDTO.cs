using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class UpdatePlaylistDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Audience Audience { get; set; }
    }
}