using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Service;

public class UpdateServiceCommand : IRequest
{
    public ServiceUpdatedEvent ServiceUpdatedEvent { get; set; }
}
