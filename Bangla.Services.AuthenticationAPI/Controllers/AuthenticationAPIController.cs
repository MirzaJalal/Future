using Bangla.MessageBus;
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
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public AuthenticationAPIController(IAuthService authService, 
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _authService = authService;
            _resposne = new();
            _messageBus = messageBus;
            _configuration = configuration;
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

            await _messageBus.SendMessageAsync(registrationDto.Email, _configuration["AzureServiceBus:RegistrationUserQueueName"]);
            
            return Ok(_resposne);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            LoginResponseDto loginResponse = await _authService.Login(loginRequestDto);

            if (loginResponse.User == null)
            {
                _resposne.IsSuccess = false;
                _resposne.Message = "Username or Password is incorrect!";

                return BadRequest(_resposne);
            }

            _resposne.Result = loginResponse;
            _resposne.IsSuccess = true;
            return Ok(_resposne);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            bool assignRoleSuccessfull = await _authService.AssignToRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper());

            if (!assignRoleSuccessfull)
            {
                _resposne.IsSuccess = false;
                _resposne.Message = "Error Encountered when assiging role!";

                return BadRequest(_resposne);
            }

            return Ok(_resposne);
        }
    }
}
