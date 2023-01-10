namespace AuthService.Models
{
    public class SpotifyModel
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }

        public SpotifyModel(string accessToken, string refreshToken) 
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
