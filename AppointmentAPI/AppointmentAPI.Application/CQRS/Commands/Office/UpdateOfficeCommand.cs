using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Office;

public class UpdateOfficeCommand : IRequest
{
    public OfficeUpdatedEvent? OfficeUpdatedEvent { get; set; }
}
