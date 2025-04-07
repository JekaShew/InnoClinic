using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Mappers.PatientMappers;

public class PatientMappers : Profile
{
    public PatientMappers()
    {
        CreateMap<Patient, PatientInfoDTO>();

        CreateMap<Patient, PatientTableInfoDTO>(); 
        
        CreateMap<PatientForCreateDTO, Patient>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
        
        CreateMap<PatientForUpdateDTO, Patient>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
    }
}
