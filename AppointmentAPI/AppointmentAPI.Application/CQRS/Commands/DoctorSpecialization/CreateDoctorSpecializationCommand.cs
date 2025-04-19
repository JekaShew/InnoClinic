using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;

public class CreateDoctorSpecializationCommand : IRequest
{
    public DoctorSpecializationCreatedEvent DoctorSpecializationCreatedEvent { get; set; }
}
