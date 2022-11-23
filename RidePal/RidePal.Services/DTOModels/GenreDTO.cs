using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class GenreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}