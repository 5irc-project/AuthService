namespace AuthService.Models
{
    public class SpotifyModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public SpotifyModel(string accessToken, string refreshToken) 
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
