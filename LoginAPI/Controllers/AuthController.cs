using LoginAPI.Models.DTOs;
using LoginAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly LoginAPI.Data.LoginDbContext _context;

        public AuthController(IAuthService authService, LoginAPI.Data.LoginDbContext context)
        {
            _authService = authService;
            _context = context;
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

        [HttpPut("cambiar-password")]
        [Authorize]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDto dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var usuario = await _context.Usuarios.FindAsync(int.Parse(userIdString));
            if (usuario == null) return NotFound();

            if (usuario.ContrasenaHash != dto.PasswordActual)
            {
                return BadRequest(new { mensaje = "La contraseña actual es incorrecta." });
            }

            usuario.ContrasenaHash = dto.NuevaPassword;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Contraseña actualizada exitosamente" });
        }

        public class CambiarPasswordDto
        {
            public string PasswordActual { get; set; }
            public string NuevaPassword { get; set; }
        }
    }
}
