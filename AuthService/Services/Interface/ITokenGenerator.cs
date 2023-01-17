using AuthService.DTO;

namespace AuthService.Services.Interface
{
    public interface ITokenGenerator
    {
        public string GenerateToken(UserLoggedDto user);
    }
}
