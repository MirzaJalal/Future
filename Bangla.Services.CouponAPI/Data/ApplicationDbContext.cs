using Bangla.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.CouponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<Coupon> Coupons { get; set; } // Plural coupons will be the table name
    }
}
