using AutoMapper;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Mappers.SpecializationMappers;

public class SpecializationMappers : Profile
{
    public SpecializationMappers() 
    {
        CreateMap<Specialization, SpecializationInfoDTO>();
        CreateMap<Specialization, SpecializationTableInfoDTO>();
        CreateMap<SpecializationForCreateDTO, Specialization>()
            .ForMember(s => s.Id, opt => opt.Ignore())
            .ForMember(s => s.DoctorSpecializations, opt => opt.Ignore());
        CreateMap<SpecializationForUpdateDTO, Specialization>();

        CreateMap<SpecializationCreatedEvent, Specialization>();
        CreateMap<SpecializationUpdatedEvent, Specialization>();
        CreateMap<SpecializationDeletedEvent, Specialization>();
        CreateMap<SpecializationCheckConsistancyEvent, Specialization>();
    }
}
