namespace Bangla.Services.OrderAPI.Models.Dtos
{
    public class StripeRequestUrl
    {
        public string StipeSessionId { get; set; }
        public string StripeSesstionUrl { get; set; }
        public string ApprovedUrl { get; set; }
        public string CalcelUrl { get; set; }
        public OrderHeaderDto OrderHeaderDto { get; set; }
    }
}
