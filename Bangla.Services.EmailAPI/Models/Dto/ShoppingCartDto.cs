namespace Bangla.Services.EmailAPI.Models.Dto
{
    public class ShoppingCartDto
    {
        public ShoppingCartHeaderDto CartHeader { get; set; }
        public List<ShoppingCartDetailsDto>? CartDetails { get; set; }
    }
}
