using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Office;

 public class DeleteOfficeCommand : IRequest
{
    public Guid Id { get; set; }
}
