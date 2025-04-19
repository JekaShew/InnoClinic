using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;

public class DeleteDoctorSpecializationCommand : IRequest
{
    public Guid Id { get; set; }
}
