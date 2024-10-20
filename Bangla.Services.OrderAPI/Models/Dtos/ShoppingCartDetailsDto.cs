namespace Bangla.Services.OrderAPI.Models.Dtos
{
    public class ShoppingCartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public ShoppingCartHeaderDto? ShoppingCartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
