using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Service;

public class DeleteServiceCommand : IRequest
{
    public Guid Id { get; set; }
}
