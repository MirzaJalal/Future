using Bangla.Services.AuthenticationAPI.Models.Dto;
using Bangla.Services.AuthenticationAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Bangla.Services.AuthenticationAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _resposne;

        public AuthenticationAPIController(IAuthService authService)
        {
            _authService = authService;
            _resposne = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequestDto registrationDto)
        {
            var errorMessage = await _authService.Register(registrationDto);
            
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _resposne.IsSuccess = false;
                _resposne.Message = errorMessage;
                return BadRequest(_resposne);
            }
            return Ok(_resposne);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            LoginResponseDto loginResponse = await _authService.Login(loginRequestDto);

            if(loginResponse is null)
            {
                _resposne.IsSuccess = false;
                _resposne.Message = "Username or Password is incorrect!";

                return BadRequest(_resposne);
            }

            _resposne.Result = loginResponse;
            _resposne.IsSuccess = true;
            return Ok(_resposne);
        }
    }
}
