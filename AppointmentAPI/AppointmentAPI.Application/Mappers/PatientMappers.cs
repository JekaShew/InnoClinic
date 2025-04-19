using AppointmentAPI.Domain.Data.Models;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.PatientEvents;

namespace AppointmentAPI.Application.Mappers;

public class PatientMappers : Profile
{
    public PatientMappers()
    {
        CreateMap<PatientCreatedEvent, Patient>();
        CreateMap<PatientUpdatedEvent, Patient>();
        CreateMap<PatientDeletedEvent, Patient>();
        CreateMap<PatientCheckConsistancyEvent, Patient>();
    }
}
