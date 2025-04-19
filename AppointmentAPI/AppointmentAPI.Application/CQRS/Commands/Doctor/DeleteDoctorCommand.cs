using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Doctor;

public class DeleteDoctorCommand : IRequest
{
    public Guid Id { get; set; }
}
