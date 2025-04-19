using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Office;

public class CreateOfficeCommand : IRequest
{
    public OfficeCreatedEvent? OfficeCreatedEvent { get; set; }
}
