using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service.IService;
using Future.Bangla.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Future.Bangla.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(
            IProductService productService, 
            ILogger<HomeController> logger,
            ICartService cartService)
        {
            _productService = productService;
            _logger = logger;
            _cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = new List<ProductDto>();

            ResponseDto? response = await _productService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto product = new ProductDto();

            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            ShoppingCartDto shoppingCartDto = new() 
            {
                CartHeader =  new ShoppingCartHeaderDto()
                {
                    UserId = User.Claims
                                .Where(u => u.Type == JwtRegisteredClaimNames.Sub)?
                                .FirstOrDefault()?.Value
                }
            };

            ShoppingCartDetailsDto shoppingCartDetailsDto = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };

            List<ShoppingCartDetailsDto> shoppingCartDetailsDtos = new List<ShoppingCartDetailsDto>() { shoppingCartDetailsDto };
            shoppingCartDto.CartDetails = shoppingCartDetailsDtos;

            ProductDto product = new ProductDto();

            ResponseDto? response = await _cartService.UpsertCartAsync(shoppingCartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item is added to the cart";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }


        [Authorize(Roles = SD.RoleAdmin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
