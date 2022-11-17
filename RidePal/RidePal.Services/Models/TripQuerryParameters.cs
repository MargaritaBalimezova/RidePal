using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Services.Models
{
    public class TripQuerryParameters
    {
        [Required]
        public string DepartCountry { get; set; }
        [Required]
        public string ArriveCountry { get; set; }
        [Required]
        public string DepartCity { get; set; }
        [Required]
        public string ArriveCity { get; set; }

        public string DepartAddress { get; set; }

        public string ArriveAddress { get; set; }
    }
}
