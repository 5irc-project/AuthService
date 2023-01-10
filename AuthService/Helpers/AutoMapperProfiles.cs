using AuthService.DTO;
using AuthService.Models;
using AutoMapper;

namespace AuthService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<SpotifyModel, LoginDto>();
        }
    }
}
