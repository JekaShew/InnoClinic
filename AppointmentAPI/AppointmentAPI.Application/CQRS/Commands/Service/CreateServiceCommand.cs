using CommonLibrary.RabbitMQEvents.ServiceEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Service;

public class CreateServiceCommand : IRequest
{
    public ServiceCreatedEvent? ServiceCreatedEvent { get; set; }
}

