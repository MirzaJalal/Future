using AutoMapper;
using Bangla.Services.OrderAPI.Data;
using Bangla.Services.OrderAPI.Models;
using Bangla.Services.OrderAPI.Models.Dtos;
using Bangla.Services.OrderAPI.Utility;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context,
            IMapper mapper,
            IProductService productService,
            ILogger<OrdersController> logger)
        {
            _responseDto = new ResponseDto();
            _context = context;
            _mapper = mapper;
            _productService = productService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] ShoppingCartDto shoppingCartDto)
        {
            try
            {
                var orderHeaderDto = _mapper.Map<OrderHeaderDto>(shoppingCartDto.CartHeader);

                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = OrderUtility.Status.Pending.ToString();
                orderHeaderDto.OrderDetails = _mapper.Map<List<OrderDetailsDto>>(shoppingCartDto.CartDetails);

                EntityEntry<OrderHeader> orderHeader = _context.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto));
                await _context.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderHeader.Entity.OrderHeaderId;
                _responseDto.Result = orderHeader.Entity;

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
                var domain = "http://localhost:4242";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                    {
                      new SessionLineItemOptions
                      {
                        // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                        Price = "",
                        Quantity = 1,
                      },
                    },
                    Mode = "payment",
                    SuccessUrl = domain + "/success.html",
                    CancelUrl = domain + "/cancel.html",
                };
                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("Location", session.Url);
            }
            catch(Exception ex)
            {
                _responseDto.Message = ex.Message; 
                _responseDto.IsSuccess = false;

                _logger.LogInformation($"Error: {ex.Message}");
            }

            return _responseDto;
        }
    }
}
