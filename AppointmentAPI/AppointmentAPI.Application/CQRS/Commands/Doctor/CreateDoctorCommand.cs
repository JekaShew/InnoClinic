using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Doctor;

public class CreateDoctorCommand : IRequest
{
    public DoctorCreatedEvent? DoctorCreatedEvent { get; set; }
}
