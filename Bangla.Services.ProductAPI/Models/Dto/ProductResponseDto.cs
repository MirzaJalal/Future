namespace Bangla.Services.ProductAPI.Models.Dto
{
    public class ProductResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
