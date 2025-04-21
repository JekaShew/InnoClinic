using CommonLibrary.RabbitMQEvents.PatientEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Patient;

public class CheckPatientConsistancyCommand : IRequest
{
    public PatientCheckConsistancyEvent? PatientCheckConsistancyEvent { get; set; }
}
