using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ventaVehiculosModels.Models
{
    public class dateUsersModels
    {
        [Display(Name = "usuario")]
        [Required]
        [StringLength(10, ErrorMessage = "El usuario supera la cantidad de caracteres permitidos, valide e intente nuevamente")]
        public string user { get; set; }

        [Display(Name = "clave")]
        [Required]
        [StringLength(8, ErrorMessage = "Clave supera la cantidad de caracteres permitidos, valide e intente nuevamente")]
        [DataType(DataType.Password)]
        public string pass { get; set; }
    }
}