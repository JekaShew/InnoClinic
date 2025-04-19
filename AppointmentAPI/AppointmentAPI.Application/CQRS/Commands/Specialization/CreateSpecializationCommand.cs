using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Specialization;

public class CreateSpecializationCommand : IRequest
{
    public SpecializationCreatedEvent? SpecializationCreatedEvent { get; set; }
}
