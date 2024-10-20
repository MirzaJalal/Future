using Bangla.Services.OrderAPI.Models.Dtos;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Builder.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {

            HttpClient client = _httpClientFactory.CreateClient("Product"); // create client
            var response = await client.GetAsync("/api/products");

            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());

            if (responseDto != null && responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                List<ProductDto> Emptyproducts = new();
            
                return Emptyproducts;
            }
        }
    }
}
