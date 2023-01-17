using AuthService.Models;

namespace AuthService.DTO
{
    public class LoginDto
    {
        public string? JwtToken { get; set; }
        public JwtTokenDto? Tokens { get; set; }

    }
}
