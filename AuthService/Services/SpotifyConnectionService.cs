using AuthService.DTO;
using AuthService.Services.Interface;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using System.Globalization;
using System.Text;
using System.Web;

namespace AuthService.Service
{
    public class SpotifyConnectionService : ISpotifyConnectionService
    {
        private readonly IConfiguration config;

        private readonly ICollection<string> scope = new[] {
            Scopes.AppRemoteControl,
            Scopes.PlaylistReadPrivate,
            Scopes.PlaylistModifyPublic,
            Scopes.PlaylistModifyPublic,
            Scopes.PlaylistReadCollaborative,
            Scopes.PlaylistReadPrivate,
            Scopes.Streaming,
            Scopes.UgcImageUpload,
            Scopes.UserFollowModify,
            Scopes.UserFollowRead,
            Scopes.UserLibraryModify,
            Scopes.UserLibraryRead,
            Scopes.UserModifyPlaybackState,
            Scopes.UserReadCurrentlyPlaying,
            Scopes.UserReadEmail,
            Scopes.UserReadPlaybackPosition,
            Scopes.UserReadPlaybackState,
            Scopes.UserReadPrivate,
            Scopes.UserReadRecentlyPlayed,
            Scopes.UserTopRead
        };

        public SpotifyConnectionService(IConfiguration configuration)
        {
            this.config = configuration;
        }

        public ICollection<string>? GetScope()
        {
            return scope;
        }

        public async Task<JwtTokenDto> getTokens(string code)
        {
            var response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(
                    config["Spotify:ClientId"],
                    config["Spotify:ClientSecret"],
                    code,
                    new Uri(config["Urls:Redirect"])
                )
            );

            return new JwtTokenDto(response.AccessToken, response.RefreshToken);
        }

        public Uri RequestLoginURI(LoginRequest.ResponseType responseType = LoginRequest.ResponseType.Code)
        {
            var loginRequest = new LoginRequest(
                  new Uri(config["Urls:Redirect"]),
                  config["Spotify:ClientId"],
                  responseType
                )
            {
                Scope = scope
            };

            return loginRequest.ToUri();
        }

        public Uri GenerateRedirectUri(JwtTokenDto spotifyTokens, string jwtToken)
        {
            StringBuilder url = new StringBuilder(config["Urls:front"] + "login");
            url.Append("?accessToken=" + spotifyTokens.AccessToken);
            url.Append("&refreshToken=" + spotifyTokens.RefreshToken);
            url.Append("&jwtToken=" + jwtToken);

            return new Uri(url.ToString());
        }
    }
}
