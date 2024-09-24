using Builder.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.ShoppingCartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<ShoppingCartDetails>? CartDetails { get; set; } // Plural coupons will be the table name
        public DbSet<ShoppingCartHeader>? CartHeaders { get; set; } // Plural coupons will be the table name
    }
}
