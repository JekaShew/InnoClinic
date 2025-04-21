using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;

public class CheckDoctorSpecializationConsistancyCommand : IRequest
{
    public DoctorSpecializationCheckConsistancyEvent? DoctorSpecializationCheckConsistancyEvent { get; set; }
}
