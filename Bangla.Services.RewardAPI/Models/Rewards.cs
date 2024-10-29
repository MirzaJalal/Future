﻿namespace Bangla.Services.RewardAPI.Models
{
    public class Rewards
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime RewardIssuedDate { get; set; }
        public int RewardActivity { get; set; }
        public int OrderId { get; set; }
    }
}
