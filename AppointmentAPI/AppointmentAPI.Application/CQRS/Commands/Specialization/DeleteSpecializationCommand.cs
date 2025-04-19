using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Specialization;

public class DeleteSpecializationCommand : IRequest
{
    public Guid Id { get; set; }
}
