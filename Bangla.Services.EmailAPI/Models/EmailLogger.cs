﻿namespace Bangla.Services.EmailAPI.Models
{
    public class EmailLogger
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime? SentEmailTime { get; set; }
    }
}
