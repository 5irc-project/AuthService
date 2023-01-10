using AuthService.Models;

namespace AuthService.DTO
{
    public class LoginDto
    {
        public string JwtToken { get; set; }
        public ITokens Tokens { get; set; }

        public LoginDto() 
        {
        }
    }
}
