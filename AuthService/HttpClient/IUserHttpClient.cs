using AuthService.DTO;
using AutoMapper;
using SpotifyAPI.Web;

namespace AuthService.HttpClient
{
    public interface IUserHttpClient
    {
        public Task<UserLoggedDto> CreateOrGetUser(PrivateUser user);
    }
}
