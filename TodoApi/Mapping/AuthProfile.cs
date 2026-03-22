using AutoMapper;
using TodoApi.API.DTOs;
using TodoApi.Models;

namespace TodoApi.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, AuthResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());
        }
    }
}
