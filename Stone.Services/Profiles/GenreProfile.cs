using AutoMapper;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;

namespace Stone.Services.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreResponseDto>();
            CreateMap<GenreRequestDto, Genre>();
        }
    }
}
