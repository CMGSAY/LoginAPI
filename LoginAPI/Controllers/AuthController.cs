using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginAPI.Models.DTOs;
using LoginAPI.Services;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result.Message == "El correo ya existe.") return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result.Token == string.Empty)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
