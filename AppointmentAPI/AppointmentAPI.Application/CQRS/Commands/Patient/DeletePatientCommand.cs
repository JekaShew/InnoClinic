using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Patient;

public class DeletePatientCommand : IRequest
{
    public Guid Id { get; set; }
}
