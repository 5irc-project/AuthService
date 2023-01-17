using AuthService.DTO;
using SpotifyAPI.Web;
using static SpotifyAPI.Web.LoginRequest;

namespace AuthService.Services.Interface
{
    public interface ISpotifyConnectionService
    {
        public Uri RequestLoginURI(ResponseType responseType);

        public ICollection<string>? GetScope();

        public Task<JwtTokenDto> getTokens(string code);

        public Uri GenerateRedirectUri(JwtTokenDto spotifyTokens, string jwtToken);
    }
}
