using Bangla.Services.AuthenticationAPI.Data;
using Bangla.Services.AuthenticationAPI.Models;
using Bangla.Services.AuthenticationAPI.Models.Dto;
using Bangla.Services.AuthenticationAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Bangla.Services.AuthenticationAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, 
           UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           ILogger<AuthService> logger
           )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
           ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool passwordMatched = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user is null && passwordMatched == false)
            {
                return new LoginResponseDto() { User = null , Token = ""};
            }

            else
            {
                // TODO: JWT Token
                UserDto userDto = new()
                {
                    Email = user.Email,
                    ID = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                };
                
                LoginResponseDto loginResponseDto = new LoginResponseDto() { User = userDto, Token = "" };
                
                return loginResponseDto;
            }
            
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if(result.Succeeded)
                {
                    var userToReurn = _context.ApplicationUsers.First(u => u.UserName == user.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReurn.Email,
                        ID = userToReurn.Id,
                        Name = userToReurn.Name,
                        PhoneNumber = userToReurn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"######## Registration Failed!! {ex.Message} ########");
            }
            return "error encountered!!";
        }
    }
}
