using Bangla.Services.AuthenticationAPI.Models.Dto;

namespace Bangla.Services.AuthenticationAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
