using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Doctor;

public class UpdateDoctorCommand : IRequest
{
    public DoctorUpdatedEvent DoctorUpdatedEvent { get; set; }
}
