using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Patient;

public class CreatePatientCommand : IRequest
{
    public PatientCreatedEvent PatientCreatedEvent { get; set; }
}
