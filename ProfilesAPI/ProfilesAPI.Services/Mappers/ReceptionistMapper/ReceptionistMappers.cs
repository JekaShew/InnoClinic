using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Mappers.ReceptionistMapper;

public class ReceptionistMappers : Profile
{
    public ReceptionistMappers()
    {
        CreateMap<Receptionist, ReceptionistInfoDTO>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo));
        
        CreateMap<Receptionist, ReceptionistTableInfoDTO>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo)); ;
        
        CreateMap<ReceptionistForCreateDTO, Receptionist>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());

        CreateMap<ReceptionistForUpdateDTO, Receptionist>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
    }
}
