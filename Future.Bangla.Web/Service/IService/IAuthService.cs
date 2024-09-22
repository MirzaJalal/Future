using Future.Bangla.Web.Models;

namespace Future.Bangla.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignToRoleAsync(RegistrationRequestDto registrationRequestDto);
    }
}
