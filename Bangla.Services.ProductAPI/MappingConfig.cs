using AutoMapper;
using Bangla.Services.ProductAPI.Models;
using Bangla.Services.ProductAPI.Models.Dto;

namespace Bangla.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
