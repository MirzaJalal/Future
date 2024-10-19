using Bangla.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.OrderAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<OrderDetails>? OrderDetails { get; set; } // Plural coupons will be the table name
        public DbSet<OrderHeader>? OrderHeaders { get; set; } // Plural coupons will be the table name
    }
}
