namespace Bangla.Services.OrderAPI.Models.Dtos
{
    public class ShoppingCartDto
    {
        public ShoppingCartHeaderDto? CartHeader { get; set; }
        public List<ShoppingCartDetailsDto>? CartDetails { get; set; }
    }
}
