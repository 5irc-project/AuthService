using AuthService.DTO;
using AutoMapper;
using SpotifyAPI.Web;

namespace AuthService.HttpClient
{
    public class UserHttpClient : IUserHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly string BASE_PATH = "";
        private readonly string CREATE_OR_GET_USER = "CreateOrGetUser";

        public UserHttpClient(IConfiguration configuration, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            BASE_PATH = configuration["Urls:UserService"];
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserLoggedDto> CreateOrGetUser(PrivateUser user)
        {
            UserDTO userDTO = _mapper.Map<UserDTO>(user);
            
            var httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<UserDTO>(BASE_PATH + CREATE_OR_GET_USER, userDTO);
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                UserLoggedDto dto = await response.Content.ReadAsAsync<UserLoggedDto>();
                return dto;
            }

            throw new Exception("HttpClient did not receive Id for User");
        }
    }
}
