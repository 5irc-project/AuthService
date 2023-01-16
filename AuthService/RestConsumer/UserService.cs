using Microsoft.AspNetCore.Mvc;
using AuthService.DTO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AuthService.RestConsumer
{
    public class UserService
    {
        private static HttpClient client = new HttpClient();

        public static async Task<UserDTO> GetUserByEmail(string email)
        {
            UserDTO dto = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7008/api/users/GetUserByEmail/" + email);
                if (response.IsSuccessStatusCode)
                {
                    dto = await response.Content.ReadAsAsync<UserDTO>();
                    return dto;
                }
            } 
            catch (Exception e)
            {
            }
            return dto;
        }

        public static async Task<void> PostUser(UserDTO user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<UserDTO>("https://localhost:7008/api/users/", user);
            response.EnsureSuccessStatusCode();

        }
    }
}
