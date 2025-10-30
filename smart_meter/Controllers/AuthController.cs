using Microsoft.AspNetCore.Mvc;
using smart_meter.Services;
using smart_meter.Model.DTOs; 

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _jwtService;

        public AuthController(AuthService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _jwtService.RegisterAsync(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = "User registered successfully" });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                var token = await _jwtService.LoginAsync(login);

                if (token == null)
                    return Unauthorized("Invalid username or password.");

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}