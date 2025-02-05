﻿using System.ComponentModel.DataAnnotations;

namespace Bangla.Services.OrderAPI.Models
{
    public class OrderHeader
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
        public string? Status { get; set; }


        public string? PaymentIntentId { get; set; }
        public string? StripePaymentSessionId { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
