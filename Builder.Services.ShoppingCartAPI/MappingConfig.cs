using AutoMapper;
using Builder.Services.ShoppingCartAPI.Models;
using Builder.Services.ShoppingCartAPI.Models.Dto;

namespace Bangla.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ShoppingCartHeader, ShoppingCartHeaderDto>().ReverseMap();
                config.CreateMap<ShoppingCartDetails, ShoppingCartDetailsDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
