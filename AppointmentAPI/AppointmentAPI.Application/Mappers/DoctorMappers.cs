using AppointmentAPI.Domain.Data.Models;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.DoctorEvents;

namespace AppointmentAPI.Application.Mappers;

public class DoctorMappers : Profile
{
	public DoctorMappers()
	{
        CreateMap<DoctorCreatedEvent, Doctor>();
        CreateMap<DoctorUpdatedEvent, Doctor>();
        CreateMap<DoctorDeletedEvent, Doctor>();
        CreateMap<DoctorCheckConsistancyEvent, Doctor>();
    }
}
