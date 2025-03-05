using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Mappers.SpecializationMappers;

public class SpecializationMappers : Profile
{
    public SpecializationMappers() 
    {
        CreateMap<Specialization, SpecializationInfoDTO>();
        CreateMap<Specialization, SpecializationTableInfoDTO>();
        CreateMap<SpecializationForCreateDTO, Specialization>();
        CreateMap<SpecializationForUpdateDTO, Specialization>();
    }
}
