using CommonLibrary.RabbitMQEvents.PatientEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Patient;

public class UpdatePatientCommand : IRequest
{
    public PatientUpdatedEvent? PatientUpdatedEvent { get; set; }
}
