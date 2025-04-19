using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Office;

public class CheckOfficeConsistancyCommand : IRequest
{
    public OfficeCheckConsistancyEvent OfficeCheckConsistancyEvent { get; set; }
}
