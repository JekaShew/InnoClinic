using AppointmentAPI.Domain.Data.Models;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.OfficeEvents;

namespace AppointmentAPI.Application.Mappers;

public class OfficeMappers : Profile
{
    public OfficeMappers()
    { 
        // Serializae BsonId string to Guid here!!!!
        CreateMap<OfficeCreatedEvent, Office>();
        CreateMap<OfficeUpdatedEvent, Office>();
        CreateMap<OfficeDeletedEvent, Office>();
        CreateMap<OfficeCheckConsistancyEvent, Office>();
    }
}
