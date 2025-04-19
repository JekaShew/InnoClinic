using AppointmentAPI.Domain.Data.Models;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;

namespace AppointmentAPI.Application.Mappers;

public class DoctorSpecializationMappers : Profile
{
    public DoctorSpecializationMappers()
    {
        CreateMap<DoctorSpecializationCreatedEvent, DoctorSpecialization>();
        CreateMap<DoctorSpecializationUpdatedEvent, DoctorSpecialization>();
        CreateMap<DoctorSpecializationDeletedEvent, DoctorSpecialization>();
        CreateMap<DoctorSpecializationCheckConsistancyEvent, DoctorSpecialization>();
    }
}
