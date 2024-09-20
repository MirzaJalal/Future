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
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(ApplicationDbContext context, 
           UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           IJwtTokenService jwtTokenService,
           ILogger<AuthService> logger
           )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<bool> AssignToRole(string email, string role)
        {
            try
            {
                ApplicationUser? user = _context.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

                if (user is not null)
                {
                    // Check if the role exists
                    var roleExists = await _roleManager.RoleExistsAsync(role);

                    if (!roleExists)
                    {
                        // Create the role if it doesn't exist
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    // Assign the role to the user
                     await _userManager.AddToRoleAsync(user, role);

                     return true;
                }

                return false;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning role {Role} to user {Email}", role, email);

                return false;
            }
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
           ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool passwordMatched = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user is null || passwordMatched == false)
            {
                return new LoginResponseDto() { User = null , Token = ""};
            }

            else
            {
                // JWT Token Generator
                IList<string> roles = await _userManager.GetRolesAsync(user); // helper method from IdentityUser
                string token = _jwtTokenService.GenerateToken(user, roles);

                UserDto userDto = new()
                {
                    Email = user.Email,
                    ID = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                };
                
                LoginResponseDto loginResponseDto = new LoginResponseDto() { User = userDto, Token = token };
                
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
