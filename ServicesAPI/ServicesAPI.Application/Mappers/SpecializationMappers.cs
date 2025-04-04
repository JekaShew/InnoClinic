using AutoMapper;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.Mappers;

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
