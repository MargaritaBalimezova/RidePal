using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Services.Models
{
    public class TripQuerryParameters
    {
        public string DepartCountry { get; set; }

        public string ArriveCountry { get; set; }

        public string DepartCity { get; set; }

        public string ArriveCity { get; set; }

        public string DepartAddress { get; set; }

        public string ArriveAddress { get; set; }
    }
}
