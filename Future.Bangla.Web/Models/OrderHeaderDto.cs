﻿namespace Future.Bangla.Web.Models
{
    public class OrderHeaderDto
    {
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }

        // Properties for Stripe Payment
        public string? PaymentIntentId { get; set; }
        public string? StripePaymentSessionId { get; set; }
        public List<OrderDetailsDto> OrderDetails { get; set; }
    }
}
