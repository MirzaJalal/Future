using Bangla.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.EmailAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}
