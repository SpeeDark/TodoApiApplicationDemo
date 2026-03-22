using AutoMapper;

using TodoApi.Models;
using TodoApi.API.DTOs;

namespace TodoApi.Mapping
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            // Request -> Entity
            CreateMap<AddTodoDto, Todo>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore());

            // Entity -> Response
            CreateMap<Todo, TodoResponseDto>()
                .ForMember(
                    destination => destination.Category,
                    options => options.MapFrom(source => 
                        source.Category != null ? source.Category : null));
        }
    }
}
