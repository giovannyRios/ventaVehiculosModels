using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ventaVehiculosModels.Models.DTOs
{
    public class carDTO
    {

        public int _IdCar { get; set; }

        [Display(Name = "Marca")]
        public string _brand { get; set; }

        [Display(Name = "Capacidad personas")]
        public int _people_capacity { get; set; }

        [Display(Name = "Tipo transmisión")]
        public string _transmission_type { get; set; }
    }
}