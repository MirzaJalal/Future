using Builder.Services.ShoppingCartAPI.Models.Dto;

namespace Builder.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
