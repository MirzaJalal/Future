using Bangla.Services.AuthenticationAPI.Models;

namespace Bangla.Services.AuthenticationAPI.Service.IService
{
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser application);
    }
}
