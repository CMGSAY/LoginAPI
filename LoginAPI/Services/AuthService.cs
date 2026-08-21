using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginAPI.Data;
using LoginAPI.Models;
using LoginAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LoginAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly LoginDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(LoginDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Usuarios
                .Include(u => u.UsuarioRols)
                    .ThenInclude(ur => ur.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.CorreoInstitucional == request.Correo );

            if (user == null || user.ContrasenaHash != request.Password)
            {
                return new LoginResponseDto
                {
                    Message = "Correo o contraseña incorrectos."
                };
            }

            if (user.Estado?.Trim().ToLower() != "activo")

            {
                return new LoginResponseDto
                {
                    Message = "Usuario se encuentra inactivo."
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.CorreoInstitucional),
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString())
            };

            foreach (var ur in user.UsuarioRols)
            {
                if (ur.IdRolNavigation != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, ur.IdRolNavigation.NombreRol));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Message = "Inicio de sesión exitoso."
            };
            
        }
    }
}
