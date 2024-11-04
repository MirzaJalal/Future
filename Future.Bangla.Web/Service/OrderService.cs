using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service.IService;
using Future.Bangla.Web.Utility;
using static Future.Bangla.Web.Utility.SD;

namespace Future.Bangla.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateOrderAsync(ShoppingCartDto shoppingCartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = shoppingCartDto,
                Url = SD.OrderAPIBase + "/api/v1/orders/CreateOrder",
            });
        }

        public async Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderAPIBase + "/api/v1/orders/CreateStripeCheckoutSession",
            });
        }

        public async Task<ResponseDto?> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Data = orderId,
                Url = SD.OrderAPIBase + "/api/v1/orders/GetOrder/"+orderId,
            });
        }

        public async Task<ResponseDto?> GetOrders(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.OrderAPIBase + "/api/v1/orders/GetOrders/"+userId,
            });
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderHeaderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = newStatus,
                Url = SD.OrderAPIBase + "/api/v1/orders/UpdateOrderStatus"+ orderHeaderId,
            });
        }

        public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrderAPIBase + "/api/v1/orders/ValidateStripeSession",
            });
        }
    }
}
