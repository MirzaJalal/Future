using AutoMapper;
using Bangla.MessageBus;
using Bangla.Services.OrderAPI.Data;
using Bangla.Services.OrderAPI.Models;
using Bangla.Services.OrderAPI.Models.Dtos;
using Bangla.Services.OrderAPI.Utility;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace Bangla.Services.OrderAPI.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ResponseDto _responseDto;
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        IConfiguration _configuration;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context,
            IMapper mapper,
            IProductService productService,
            IMessageBus messageBus,
            IConfiguration configuration,
            ILogger<OrdersController> logger)
        {
            _responseDto = new ResponseDto();
            _context = context;
            _mapper = mapper;
            _messageBus = messageBus;
            _configuration = configuration;
            _productService = productService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] ShoppingCartDto shoppingCartDto)
        {

            try
            {
            string incomingJson = JsonConvert.SerializeObject(shoppingCartDto, Formatting.Indented);
                var orderHeaderDto = _mapper.Map<OrderHeaderDto>(shoppingCartDto.CartHeader);

                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = OrderUtility.Status.Pending.ToString();
                orderHeaderDto.OrderDetails = _mapper.Map<List<OrderDetailsDto>>(shoppingCartDto.CartDetails);

                EntityEntry<OrderHeader> orderHeader = _context.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto));
                await _context.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderHeader.Entity.OrderHeaderId;
                _responseDto.Result = orderHeaderDto;

                _logger.LogInformation($"Oreder response is ##########{_responseDto}");

                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
                _logger.LogInformation(ex.ToString());

                return _responseDto;
            }
        }

        [Authorize]
        [HttpPost("CreateStripeCheckoutSession")]
        public async Task<ResponseDto> Create([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {
                var options = new SessionCreateOptions
                {

                    Mode = "payment",
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancellationUrl,
                    LineItems = new List<SessionLineItemOptions>()
                };

                var discountObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon = stripeRequestDto.OrderHeaderDto.CouponCode
                    }
                };

                // As we have list of products in the order
                foreach(var item in stripeRequestDto.OrderHeaderDto.OrderDetails)
                {
                    var lineItemOptions = new SessionLineItemOptions
                    {
                      PriceData =  new SessionLineItemPriceDataOptions
                      {
                        UnitAmount = (long)(item.Price*100), // 20.99 = 2099 
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                      },
                      Quantity = item.Count
                    };

                    options.LineItems.Add(lineItemOptions);
                }

                if(stripeRequestDto.OrderHeaderDto.Discount > 0)
                {
                    options.Discounts = discountObj;    
                }

                var service = new SessionService();
                Session session = service.Create(options); // stripe session

                stripeRequestDto.StripeSessionUrl = session.Url;
                stripeRequestDto.CancellationUrl = session.CancelUrl;

                var orderHeader = _context.OrderHeaders.First(o => o.OrderHeaderId == stripeRequestDto.OrderHeaderDto.OrderHeaderId);
                orderHeader.StripePaymentSessionId = session.Id;
                _context.SaveChanges();

                _responseDto.Result = stripeRequestDto;
            }
            catch(Exception ex)
            {
                _responseDto.Message = ex.Message; 
                _responseDto.IsSuccess = false;

                _logger.LogInformation($"Error: {ex.Message}");
            }

            return _responseDto;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                var orderHeader = _context.OrderHeaders.First(o => o.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripePaymentSessionId); // stripe session

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if(paymentIntent.Status == "succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = OrderUtility.Status.Approved.ToString();

                    _context.SaveChanges();

                    RewardsDto rewardsDto = new RewardsDto() 
                    { 
                        OrderId = orderHeader.OrderHeaderId,
                        RewardActivity = (int)orderHeader.OrderTotal,
                        UserId = orderHeader.UserId
                    };

                    // sending rewardsDto to the azure Queue which is a multiple subcriber based bus
                    await _messageBus.SendMessageAsync(rewardsDto, _configuration["AzureServiceBus:EmailShoppingCart_QueueName"]);

                    _responseDto.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;

                _logger.LogInformation($"Error: {ex.Message}");
            }

            return _responseDto;
        }
    }
}
