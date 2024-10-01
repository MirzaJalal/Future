using Builder.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.ShoppingCartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<ShoppingCartDetails>? cartDetails { get; set; } // Plural coupons will be the table name
        public DbSet<ShoppingCartHeader>? cartHeaders { get; set; } // Plural coupons will be the table name
    }
}
