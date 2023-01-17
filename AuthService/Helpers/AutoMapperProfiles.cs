using AuthService.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using SpotifyAPI.Web;

namespace AuthService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<PrivateUser, UserDTO>()
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images.First().Url : ""));
        }
    }
}
