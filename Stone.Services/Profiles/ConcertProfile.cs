using AutoMapper;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Entities.Info;

namespace Stone.Services.Profiles
{
    public class ConcertProfile: Profile
    {
        public ConcertProfile()
        {
            CreateMap<ConcertInfo, ConcertResponseDto>(); //origen -> destino
            CreateMap<Concert, ConcertResponseDto>()
                .ForMember(d => d.DateEvent, o => o.MapFrom(x => x.DateEvent.ToShortDateString()))//setear atributo DateEvent de ConcertResponse
                .ForMember(d => d.TimeEvent, o => o.MapFrom(x => x.DateEvent.ToShortTimeString()))//setear atributo TimeEvent de ConcertResponse
                .ForMember(d => d.Status, o => o.MapFrom(x => x.Active ? "Activo" : "Inactivo"));//setear atributo Status de ConcertResponse
            CreateMap<ConcertRequestDto, Concert>() //destino -> origen
                .ForMember(d => d.DateEvent, o => o.MapFrom(x => Convert.ToDateTime($"{x.DateEvent} {x.TimeEvent}")))
                .ForMember(d => d.ImageUrl, options => options.Ignore()); //ignora este mapeo
        }
    }
}
