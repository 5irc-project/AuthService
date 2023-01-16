using Microsoft.AspNetCore.Mvc;
using AuthService.DTO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AuthService.RestConsumer
{
    public class UserService
    {
        private static HttpClient client = new HttpClient();
        private static readonly string BASE_PATH = "https://localhost:7008/api/users/";

        public static async Task<UserLoggedDto> CreateOrGetUser(UserDTO user)
        {
            UserLoggedDto dto = null;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync<UserDTO>(BASE_PATH + "CreateOrGetUser", user);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    dto = await response.Content.ReadAsAsync<UserLoggedDto>();
                    return dto;
                }
            }
            catch (Exception e)
            {

            }
            return dto;
        }
    }
}
