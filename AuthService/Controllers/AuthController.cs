using AuthService.DTO;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpotifyAPI.Web;
using Swan;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.DTO;
using AuthService.RestConsumer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        public AuthController(IMapper mapper, IConfiguration config)
        {
            this.mapper = mapper;
            this.config = config;
        }

        [HttpGet("/auth")]
        public string Auth()
        {
            var loginRequest = new LoginRequest(
              new Uri(config["Urls:Redirect"]),
              config["Spotify:ClientId"],
              LoginRequest.ResponseType.Code
            )
            {
                Scope = new[] { Scopes.UserTopRead,
                                Scopes.UserReadPlaybackState,
                                Scopes.UserModifyPlaybackState,
                                Scopes.UserReadCurrentlyPlaying,
                                Scopes.PlaylistReadPrivate,
                                Scopes.PlaylistReadCollaborative,
                                Scopes.Streaming,
                                Scopes.UserReadEmail,
                                Scopes.AppRemoteControl }
            };
            var uri = loginRequest.ToUri();
            // Redirect user to uri via your favorite web-server
            return uri.ToString();
        }

        [HttpGet("/redirect")]
        public async Task<LoginDto> AuthRedirect(String code)
        {
            var response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(config["Spotify:ClientId"],
                                                  config["Spotify:ClientSecret"],
                                                  code,
                                                  new Uri(config["Urls:Redirect"]))
              );

            var spotify = new SpotifyClient(response.AccessToken);
            var user = await spotify.UserProfile.Current();

            // DTO to send to userservie to register a user
            UserDTO userDto = new UserDTO();
            userDto.Email = user.Email;
            userDto.Nom = user.DisplayName;
            userDto.ProfilePictureUrl = user.Images.First().Url;

            // Create or get user
            UserLoggedDto dto = await UserService.CreateOrGetUser(userDto);

            // Generate JWT
            var jwtString = GenerateJwtToken(dto);
            SpotifyModel tokens = new SpotifyModel(response.AccessToken, response.RefreshToken);

            LoginDto spotifyDto = new LoginDto();
            spotifyDto.JwtToken = jwtString;
            spotifyDto.Tokens = tokens;

            Response.Redirect($"http://localhost/login?accessToken={spotifyDto.Tokens.AccessToken}&refreshToken={spotifyDto.Tokens.RefreshToken}&jwtToken={spotifyDto.JwtToken}");

            return spotifyDto;
            // Also important for later: response.RefreshToken
        }

        private string GenerateJwtToken(UserLoggedDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("displayName", user.Nom),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
