using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.Mappers;

public class SpecializationMappers : Profile
{
    public SpecializationMappers()
    {
        CreateMap<Specialization, SpecializationInfoDTO>();

        CreateMap<Specialization, SpecializationTableInfoDTO>()
            .ForSourceMember(src => src.ServiceCategorySpecializations, opt => opt.DoNotValidate());
        
        CreateMap<SpecializationForCreateDTO, Specialization>();
        
        CreateMap<SpecializationForUpdateDTO, Specialization>()
            .ForMember(dest => dest.Id, opt=> opt.Ignore());

        CreateMap<SpecializationCreatedEvent, Specialization>().ReverseMap();
        CreateMap<SpecializationUpdatedEvent, Specialization>().ReverseMap();
        CreateMap<SpecializationDeletedEvent, Specialization>().ReverseMap();
    }
}
