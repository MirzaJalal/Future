using Microsoft.AspNetCore.Identity;

namespace Bangla.Services.AuthenticationAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
