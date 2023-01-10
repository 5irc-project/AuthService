using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("/test")]
        public String test()
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
        public async Task<string> TestRedirect(String code)
        {
            Console.WriteLine(code);
            var response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest("5212c3ac72cc47dab1ac2868861a5c3c",
                                                  "99cfaa8b04664f6989d4b2cf52763fe8",
                                                  code,
                                                  new Uri("https://localhost:7091/redirect"))
              );

            var spotify = new SpotifyClient(response.AccessToken);
            Response.Redirect("https://localhost:7091");
            return null;
            // Also important for later: response.RefreshToken
        }
    }
}
