namespace Future.Bangla.Web.Models
{
    public class ShoppingCartDto
    {
        public ShoppingCartHeaderDto CartHeader { get; set; }
        public List<ShoppingCartDetailsDto>? CartDetails { get; set; }
    }
}
