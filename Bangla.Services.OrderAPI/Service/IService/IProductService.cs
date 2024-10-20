
using Bangla.Services.OrderAPI.Models.Dtos;

namespace Builder.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
