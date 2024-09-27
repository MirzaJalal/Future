using AutoMapper;
using Bangla.Services.ShoppingCartAPI.Data;
using Builder.Services.ShoppingCartAPI.Models;
using Builder.Services.ShoppingCartAPI.Models.Dto;
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
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;
        private readonly ILogger<ShoppingCartController> _logger;
        public ShoppingCartController(ApplicationDbContext context,
            IMapper mapper,
            ILogger<ShoppingCartController> logger)
        {
            _context = context;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _logger = logger;

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
    }
}

