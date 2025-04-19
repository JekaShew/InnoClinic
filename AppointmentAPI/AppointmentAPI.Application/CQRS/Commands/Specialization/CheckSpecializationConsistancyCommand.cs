using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Specialization;

public class CheckSpecializationConsistancyCommand : IRequest
{
    public SpecializationCheckConsistancyEvent SpecializationCheckConsistancyEvent{ get; set; }
}
