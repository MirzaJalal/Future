using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bangla.Services.AuthenticationAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Registration()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
