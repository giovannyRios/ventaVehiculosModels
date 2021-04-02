using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ventaVehiculosModels.Models.DTOs
{
    public class carDTO
    {
        public int _IdCar { get; set; }

        public string _brand { get; set; }

        public int _people_capacity { get; set; }

        public string _transmission_type { get; set; }
    }
}