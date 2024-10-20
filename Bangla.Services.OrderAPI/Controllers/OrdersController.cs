using AutoMapper;
using Bangla.Services.OrderAPI.Data;
using Bangla.Services.OrderAPI.Models;
using Bangla.Services.OrderAPI.Models.Dtos;
using Bangla.Services.OrderAPI.Utility;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task<ResponseDto> CreateOrder([FromBody] ShoppingCartDto shoppingCartDto)
        {
            try
            {
                var orderHeaderDto = _mapper.Map<OrderHeaderDto>(shoppingCartDto.CartHeader);

                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.OrderStatus = OrderUtility.Status.Pending.ToString();
                orderHeaderDto.OrderDetails = _mapper.Map<List<OrderDetails>>(shoppingCartDto.CartDetails);

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
    }
}
