using Builder.Services.ShoppingCartAPI.Models.Dto;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Builder.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCoupon(string couponCode)
        {

            HttpClient client = _httpClientFactory.CreateClient("Coupon"); // create client
            var response = await client.GetAsync($"/api/coupons/GetByCode/{couponCode}");

            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());

            if (responseDto != null && responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }
            else
            {
                CouponDto Emptyproducts = new();

                return Emptyproducts;
            }
        }
    }
}
