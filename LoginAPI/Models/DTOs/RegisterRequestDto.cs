namespace LoginAPI.Models.DTOs
{
    public class RegisterRequestDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdRol { get; set; } 
    }
}
