namespace AuthService.DTO
{
    public class JwtTokenDto
    { 
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
            
        public JwtTokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
