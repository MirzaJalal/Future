using Bangla.Services.AuthenticationAPI.Models.Dto;

namespace Bangla.Services.AuthenticationAPI.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
