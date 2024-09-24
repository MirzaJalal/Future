namespace Builder.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public ShoppingCartHeaderDto CartHeader { get; set; }
        public List<ShoppingCartDetailsDto>? CartDetails { get; set; }
    }
}
