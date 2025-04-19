using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Specialization;

public class UpdateSpecializationCommand : IRequest
{
    public SpecializationUpdatedEvent SpecializationUpdatedEvent { get; set; }
}
