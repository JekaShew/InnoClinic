using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;

public class UpdateDoctorSpecializationCommand : IRequest
{
    public DoctorSpecializationUpdatedEvent? DoctorSpecializationUpdatedEvent { get; set; }
}
