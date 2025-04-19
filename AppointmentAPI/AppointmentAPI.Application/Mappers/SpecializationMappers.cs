using AppointmentAPI.Domain.Data.Models;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;

namespace AppointmentAPI.Application.Mappers;

public class SpecializationMappers : Profile
{
    public SpecializationMappers()
    {
        CreateMap<SpecializationCreatedEvent, Specialization>();
        CreateMap<SpecializationUpdatedEvent, Specialization>();
        CreateMap<SpecializationDeletedEvent, Specialization>();
        CreateMap<SpecializationCheckConsistancyEvent, Specialization>();
    }
}
