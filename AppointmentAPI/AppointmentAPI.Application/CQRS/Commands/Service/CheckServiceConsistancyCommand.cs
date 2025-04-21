using CommonLibrary.RabbitMQEvents.ServiceEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Service;

public class CheckServiceConsistancyCommand : IRequest
{
    public ServiceCheckConsistancyEvent? ServiceCheckConsistancyEvent { get; set; }
}
