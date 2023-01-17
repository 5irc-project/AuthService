using AuthService.DTO;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpotifyAPI.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Services.Interface;
using AuthService.HttpClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly ISpotifyConnectionService spotifyConnectionService;
        private readonly IUserHttpClient userHttpClient;
        private readonly ITokenGenerator tokenGenerator;
        public AuthController(
            IConfiguration config,
            ISpotifyConnectionService spotifyConnectionService,
            IUserHttpClient userHttpClient,
            ITokenGenerator tokenGenerator
            )
        {
            this.config = config;
            this.spotifyConnectionService = spotifyConnectionService;
            this.userHttpClient = userHttpClient;
            this.tokenGenerator = tokenGenerator;
        }

        [HttpGet()]
        [ActionName("auth")]
        public string Auth()
        {
            // Redirect user to uri via your favorite web-server
            return spotifyConnectionService.RequestLoginURI(LoginRequest.ResponseType.Code).ToString();
        }

        [HttpGet()]
        [ActionName("redirect")]
        public async void AuthRedirect(String code)
        {
            //get token and get User
            JwtTokenDto spotifyTokens = spotifyConnectionService.getTokens(code).Result;
            var spotifyClient = new SpotifyClient(spotifyTokens.AccessToken);
            var user = await spotifyClient.UserProfile.Current();
            

            // Create or get user from UserService
            UserLoggedDto LoggedInUser = await userHttpClient.CreateOrGetUser(user);

            // Generate JWT
            var jwtString = tokenGenerator.GenerateToken(LoggedInUser);

            LoginDto spotifyDto = new LoginDto();
            spotifyDto.JwtToken = jwtString;
            spotifyDto.Tokens = spotifyTokens;

            Response.Redirect(spotifyConnectionService.GenerateRedirectUri(spotifyTokens, jwtString).ToString());
        }

        [HttpGet()]
        [ActionName("LoginWithoutSpotify")]
        public LoginDto LoginWithoutSpotify()
        {

            UserLoggedDto user = new UserLoggedDto(1, "test", "test@test.com", "test.jpg");

            // Generate JWT
            var jwtString = tokenGenerator.GenerateToken(user);

            return new LoginDto()
            {
                JwtToken = jwtString
            };
        }
    }
}
