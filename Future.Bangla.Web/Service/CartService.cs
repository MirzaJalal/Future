using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service.IService;
using Future.Bangla.Web.Utility;
using static Future.Bangla.Web.Utility.SD;

namespace Future.Bangla.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {

            _baseService = baseService;

        }

        public async Task<ResponseDto?> ApplyCouponAsync(ShoppingCartDto shoppingCartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = shoppingCartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/UseCoupon",
            });
        }

        public async Task<ResponseDto?> GetCartAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.ShoppingCartAPIBase + $"/api/cart/GetCart/{userId}",
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart",
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(ShoppingCartDto shoppingCartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = shoppingCartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/upsert",
            });
        }
    }
}
