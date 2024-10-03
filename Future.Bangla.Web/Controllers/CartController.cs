using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Future.Bangla.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoByLoggedInUser());
        }

        public async Task<IActionResult> Remove(int CartDetailsId)
        {
            string? userId = User.Claims
                .Where(u => u.Type == JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.RemoveFromCartAsync(CartDetailsId);

            if (response == null)
            {
                return View();
            }
            else
            {
                TempData["success"] = "Cart Updated!";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(ShoppingCartDto shoppingCartDto)
        {
            ResponseDto? response = await _cartService.ApplyCouponAsync(shoppingCartDto);

            if (response == null)
            {
                return View();
            }
            else
            {
                TempData["success"] = "Cart Updated!";
                return RedirectToAction(nameof(CartIndex));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EmailCart(ShoppingCartDto shoppingCartDto)
        {
            ResponseDto? response = await _cartService.EmailCartAsync(shoppingCartDto);

            if (response == null)
            {
                return View();
            }
            else
            {
                TempData["success"] = "An email will be sent to you shortly!";
                return RedirectToAction(nameof(CartIndex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(ShoppingCartDto shoppingCartDto)
        {
            shoppingCartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await _cartService.ApplyCouponAsync(shoppingCartDto);

            if (response == null)
            {
                return View();
            }
            else
            {
                TempData["success"] = "Cart Updated!";
                return RedirectToAction(nameof(CartIndex));
            }
        }

        private async Task<ShoppingCartDto> LoadCartDtoByLoggedInUser()
        {
            string? userId = User.Claims
                .Where(u => u.Type == JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.GetCartAsync(userId);

            if (response.IsSuccess is not true)
            {
                return new ShoppingCartDto();
            }
            else
            {
                ShoppingCartDto? cartDto = JsonConvert
                    .DeserializeObject<ShoppingCartDto>(Convert.ToString(response.Result));
               
                return cartDto;
            }
        }
    }
}
