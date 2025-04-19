using AppointmentAPI.Domain.Data.Models;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.ServiceEvents;

namespace AppointmentAPI.Application.Mappers;

public class ServiceMappers : Profile
{
    public ServiceMappers()
    {
        CreateMap<ServiceCreatedEvent, Service>();
        CreateMap<ServiceUpdatedEvent, Service>();
        CreateMap<ServiceDeletedEvent, Service>();
        CreateMap<ServiceCheckConsistancyEvent, Service>();
    }
}
