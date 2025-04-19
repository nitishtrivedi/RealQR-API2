using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RealQR_API.Services;

namespace RealQR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authService.Register(request.Username, request.FirstName, request.LastName, request.Password, request.Email, request.IsUserAdmin); 
                return Ok(new {Token = token});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.Login(request.Username, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid Password")
                {
                    return Unauthorized(ex.Message);
                } 
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsUserAdmin { get; set; } = false; //Change isUserAdmin manually. Only set admins if needed
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
