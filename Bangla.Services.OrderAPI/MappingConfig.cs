using AutoMapper;
using Bangla.Services.OrderAPI.Models;
using Bangla.Services.OrderAPI.Models.Dtos;


namespace Bangla.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, ShoppingCartHeaderDto>()
                    .ForMember(dest => dest.CartTotal, o => o.MapFrom(src => src.OrderTotal)).ReverseMap();

                config.CreateMap<ShoppingCartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, o => o.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, o => o.MapFrom(src => src.Product.Price));

                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
