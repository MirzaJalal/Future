using Future.Bangla.Web.Models;
using Future.Bangla.Web.Service.IService;
using Future.Bangla.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Future.Bangla.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {

           LoginRequestDto loginRequestDto = new();

           return View(loginRequestDto);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            // Viewbag for dropdown list for roles
            ViewBag.RoleList = roleList;

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto? responseDto = await _authService.RegisterAsync(registrationRequestDto);

            ResponseDto? assignRole;

            if(responseDto != null && responseDto.IsSuccess) 
            {
                // By Default user is assigned to cutomer role
                if(string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }

                // Role is assigned if we want to set role manually
                assignRole =  await _authService.AssignToRoleAsync(registrationRequestDto);
                
                if(assignRole != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Registration Successful!";
                    return RedirectToAction(nameof(Login));
                }
            
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            // Viewbag for dropdown list for roles
            ViewBag.RoleList = roleList;

            return View();
        }

        public async Task<IActionResult> Logout()
        {

            return View();
        }
    }
}
