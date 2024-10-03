using AutoMapper;
using Bangla.MessageBus;
using Bangla.Services.ShoppingCartAPI.Data;
using Builder.Services.ShoppingCartAPI.Models;
using Builder.Services.ShoppingCartAPI.Models.Dto;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Builder.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;
        private readonly IMessageBus _messageBus;
        IConfiguration _configuration;
        private readonly ILogger<ShoppingCartController> _logger;
        public ShoppingCartController(ApplicationDbContext context,
            IProductService productService,
            ICouponService couponService,
            IMapper mapper,
            IMessageBus messageBus,
            IConfiguration configuration,
            ILogger<ShoppingCartController> logger)
        {
            _context = context;
            _productService = productService;
            _couponService = couponService;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _messageBus = messageBus;
            _configuration = configuration;
            _logger = logger;

        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                ShoppingCartDto cartDto = new()
                {
                    CartHeader = _mapper.Map<ShoppingCartHeaderDto>(_context.cartHeaders.First(u => u.UserId == userId))
                };

                var shoppingCartDetailsDto = _mapper.Map<IEnumerable<ShoppingCartDetailsDto>>(_context.cartDetails
                    .Where(u => u.CartHeaderId == cartDto.CartHeader.CartHeaderId)).ToList();

                cartDto.CartDetails = shoppingCartDetailsDto;
                
                // received the list of products from the product service using http request 
                var productListDto = await _productService.GetProducts();

                foreach(var item in cartDto.CartDetails)
                {
                    item.Product = productListDto.FirstOrDefault(p => p.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                // applying coupon to the cart total

                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var couponDto = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                    
                    if(couponDto != null && cartDto.CartHeader.CartTotal > couponDto.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= couponDto.DiscountAmount;
                        cartDto.CartHeader.Discount = couponDto.DiscountAmount;
                    }

                }

                _responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }

        [HttpPost("UseCoupon")]
        public async Task<object> ApplyCoupon([FromBody] ShoppingCartDto shoppingCartDto)
        {
            try
            {

               var cart = await _context.cartHeaders.FirstOrDefaultAsync(c => c.UserId == shoppingCartDto.CartHeader.UserId);
            
                if(cart != null)
                {
                    cart.CouponCode = shoppingCartDto.CartHeader.CouponCode;

                    //var couponDto = await _couponService.GetCoupon(shoppingCartDto.CartHeader.CouponCode);
                    //cart.Discount = couponDto.DiscountAmount;

                     _context.cartHeaders.Update(cart);
                    await _context.SaveChangesAsync();
                    _responseDto.Result= true;
                }
                else
                {
                    _responseDto.Message = "Cart not found";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message); 
                _responseDto.Message = ex.ToString();
            }

            return _responseDto;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] ShoppingCartDto shoppingCartDto)
        {
            try
            {

                var cart = await _context.cartHeaders.FirstOrDefaultAsync(c => c.UserId == shoppingCartDto.CartHeader.UserId);

                if (cart != null)
                {
                    cart.CouponCode = "";
                    _context.cartHeaders.Update(cart);
                    await _context.SaveChangesAsync();
                    _responseDto.Result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _responseDto.Message = ex.ToString();
            }

            return _responseDto;
        }

        [HttpPost("upsert")]
        public async Task<ResponseDto> UpsertCart([FromBody] ShoppingCartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _context.cartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(h => h.UserId == cartDto.CartHeader.UserId);

                // Check if CartHeader exists for the User (if not create one)
                if (cartHeaderFromDb == null)
                {
                    var shoppingCartHeader = _mapper.Map<ShoppingCartHeader>(cartDto.CartHeader);
                    _context.cartHeaders.Add(shoppingCartHeader);
                    await _context.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = shoppingCartHeader.CartHeaderId;
                    var shoppingCartDetails = _mapper.Map<ShoppingCartDetails>(cartDto.CartDetails.First());
                    _context.cartDetails.Add(shoppingCartDetails);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // if header is not null
                    // check if the details has same product
                    var existingCartDetailFromDb = await _context.cartDetails.AsNoTracking()
                    .FirstOrDefaultAsync(d => d.CartHeaderId == cartHeaderFromDb.CartHeaderId && d.ProductId == cartDto.CartDetails.First().ProductId);

                    // Check if Product already exists in the CartDetails for this header
                    if (existingCartDetailFromDb == null)
                    {
                        // If no existing entry, add new CartDetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        var shoppingCartDetails = _mapper.Map<ShoppingCartDetails>(cartDto.CartDetails.First());
                        _context.cartDetails.Add(shoppingCartDetails);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        // If product exists, update the count
                        cartDto.CartDetails.First().Count += existingCartDetailFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = existingCartDetailFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = existingCartDetailFromDb.CartDetailsId;

                        var shoppingCartDetails = _mapper.Map<ShoppingCartDetails>(cartDto.CartDetails.First());
                        _context.cartDetails.Update(shoppingCartDetails);
                        await _context.SaveChangesAsync();
                    }
                }

                _responseDto.Result = cartDto;
                _responseDto.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess=false;
            }
             return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCartDetails([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetailsResult = _context.cartDetails.First(d => d.CartDetailsId == cartDetailsId);

                int totalCartItem = _context.cartDetails.Where(c => c.CartHeaderId == cartDetailsResult.CartHeaderId).Count();
            

                _context.cartDetails.Remove(cartDetailsResult);

                if(totalCartItem == 1)
                {
                    var cartHeaderToRemove = await _context.cartHeaders
                        .FirstOrDefaultAsync(h => h.CartHeaderId == cartDetailsResult.CartHeaderId);
                    _context.cartHeaders.Remove(cartHeaderToRemove);
                }
                _responseDto.IsSuccess = true;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _responseDto.Message =  ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }

       [HttpPost("emailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] ShoppingCartDto cartDto)
        {
            try
            {
                await _messageBus.SendMessageAsync(cartDto, _configuration["AzureServiceBus:QueueName"]);
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.ToString();
            }

            return _responseDto;
        }

    }
}

