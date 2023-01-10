using AuthService.DTO;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpotifyAPI.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public String Auth()
        {
            var loginRequest = new LoginRequest(
              new Uri("https://localhost:7091/redirect"),
              "5212c3ac72cc47dab1ac2868861a5c3c",
              LoginRequest.ResponseType.Code
            )
            {
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            var uri = loginRequest.ToUri();
            // Redirect user to uri via your favorite web-server
            return uri.ToString();
        }

        [HttpGet("/redirect")]
        public async Task<SpotifyDto> AuthRedirect(String code)
        {
            Console.WriteLine("OUIOUIOUIOUI", code);
            var response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest("5212c3ac72cc47dab1ac2868861a5c3c",
                                                  "99cfaa8b04664f6989d4b2cf52763fe8",
                                                  code,
                                                  new Uri("https://localhost:7091/redirect"))
              );

            var spotify = new SpotifyClient(response.AccessToken);
            //Response.Redirect("https://localhost:7091");

            // Generate JWT
            var jwtString = GenerateJwtToken();

            SpotifyModel tokens = new SpotifyModel(response.AccessToken, response.RefreshToken);
            SpotifyDto spotifyDto = mapper.Map<SpotifyDto>(tokens);
            return spotifyDto;
            // Also important for later: response.RefreshToken
        }

        private string GenerateJwtToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "userName"),
                new Claim("fullName", "fullName"),
                new Claim("role", "userRole"),
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
