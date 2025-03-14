using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Mappers.PatientMappers;

public class PatientMappers : Profile
{
    public PatientMappers()
    {
        CreateMap<Patient, PatientInfoDTO>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo));
        
        CreateMap<Patient, PatientTableInfoDTO>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo)); 
        
        CreateMap<PatientForCreateDTO, Patient>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
        
        CreateMap<PatientForUpdateDTO, Patient>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
    }
}
