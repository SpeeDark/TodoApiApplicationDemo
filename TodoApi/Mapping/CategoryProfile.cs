using AutoMapper;

using TodoApi.Models;
using TodoApi.API.DTOs;

namespace TodoApi.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryResponseDto>();

            CreateMap<AddCategoryDto, Category>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore());

            CreateMap<Category, CategoryWithTodoListResponseDto>()
                .ForMember(dest => dest.Todos,
                    opt => opt.MapFrom(source => source.Todos));
        }
    }
}
