namespace Bangla.Services.OrderAPI.Models.Dtos
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancellationUrl { get; set; }
        public OrderHeaderDto OrderHeaderDto { get; set; }
    }
}
