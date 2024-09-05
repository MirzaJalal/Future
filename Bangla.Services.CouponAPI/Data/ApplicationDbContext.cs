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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData( new Coupon
            {
                CouponId = 1,
                CouponCode = "25OFF",
                DiscountAmount = 25,
                MinAmount = 50,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "35OFF",
                DiscountAmount = 35,
                MinAmount = 90,
            });
        }
    }
}
