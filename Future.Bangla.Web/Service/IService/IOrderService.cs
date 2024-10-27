using Future.Bangla.Web.Models;

namespace Future.Bangla.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrderAsync(ShoppingCartDto shoppingCartDto);
        Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto);
    }
}
