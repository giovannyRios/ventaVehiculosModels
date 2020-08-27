using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ventaVehiculosModels.Models.DTOs
{
    public class loginUserDTO
    {
        public long _IdUser { get; set; }
        public string _UserStr { get; set; }
        public string _Pass { get; set; }
        public bool _IsActive { get; set; }

    }
}