using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tekton.Models;

namespace Tekton.App_Start
{
    public static class AutomapperConfig
    {
        public static void Configure()
        {
            ConfigureMapping();
        }

        private static void ConfigureMapping()
        {
            //Mapper.CreateMap<ProductViewModel, Product>();
                //.ForMember(destination => destination.ProductID, options => options.MapFrom(source => source.ProductID));
        } 
    }
}