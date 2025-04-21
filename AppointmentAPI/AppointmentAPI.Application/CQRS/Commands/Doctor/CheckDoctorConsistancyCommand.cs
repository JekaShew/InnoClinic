using CommonLibrary.RabbitMQEvents.DoctorEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Doctor;

public class CheckDoctorConsistancyCommand : IRequest
{
    public DoctorCheckConsistancyEvent? DoctorCheckConsistancyEvent { get; set; }
}
