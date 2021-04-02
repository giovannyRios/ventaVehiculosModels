using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ventaVehiculosModels.Models.EntityFramework;
using ventaVehiculosModels.Models.DTOs;
using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace ventaVehiculosModels.Models.DTOs
{
    public class AutoMapperConfiguration
    {

        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg =>
              {
                  cfg.AddCollectionMappers();
                  cfg.CreateMap<loginUser, loginUserDTO>().ReverseMap();
              });
        }
    }
}