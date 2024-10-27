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
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoByLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {

            return View(await LoadCartDtoByLoggedInUser());
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(ShoppingCartDto shoppingCartDto)
        {
            ShoppingCartDto updatedCart = await LoadCartDtoByLoggedInUser();

            updatedCart.CartHeader.Phone = shoppingCartDto.CartHeader.Phone;
            updatedCart.CartHeader.Email = shoppingCartDto.CartHeader.Email;
            updatedCart.CartHeader.Name = shoppingCartDto.CartHeader.Name;

            ResponseDto response = await _orderService.CreateOrderAsync(updatedCart);

            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if(response != null && response.IsSuccess)
            {
                // Stripe Payment
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Confirmation(int orderId)
        {

            return View(orderId);
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
            ShoppingCartDto shoppingCart = await LoadCartDtoByLoggedInUser();

            string? email = User.Claims
                                .Where(u => u.Type == JwtRegisteredClaimNames.Email)?
                                .FirstOrDefault()?.Value;

            shoppingCart.CartHeader.Email = email;

            ResponseDto? response = await _cartService.EmailCartAsync(shoppingCart);

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
