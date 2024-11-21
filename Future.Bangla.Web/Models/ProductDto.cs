using System.ComponentModel.DataAnnotations;

namespace Future.Bangla.Web.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Range(0, 50)]
        public int Count { get; set; } = 1;
    }
}
