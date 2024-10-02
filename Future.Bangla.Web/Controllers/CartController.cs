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

        private async Task<ShoppingCartDto> LoadCartDtoByLoggedInUser()
        {
            string? userId = User.Claims
                .Where(u => u.Type == JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.GetCartAsync(userId);

            if (response == null)
            {
                return new ShoppingCartDto();
            }
            else
            {
                ShoppingCartDto cartDto = JsonConvert
                    .DeserializeObject<ShoppingCartDto>(Convert.ToString(response.Result));
               
                return cartDto;
            }
        }
    }
}
