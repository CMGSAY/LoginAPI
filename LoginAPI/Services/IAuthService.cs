using LoginAPI.Models.DTOs;

namespace LoginAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
