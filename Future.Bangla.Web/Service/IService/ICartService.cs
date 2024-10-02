using Future.Bangla.Web.Models;

namespace Future.Bangla.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(ShoppingCartDto shoppingCartDto);
        Task<ResponseDto?> RemoveFromCartAstnc(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(ShoppingCartDto shoppingCartDto);
    }
}
