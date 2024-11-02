using Bangla.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.RewardAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Rewards> Rewards { get; set; }
    }
}
