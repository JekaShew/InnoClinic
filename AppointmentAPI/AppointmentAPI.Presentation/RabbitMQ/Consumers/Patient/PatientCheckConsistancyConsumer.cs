using AppointmentAPI.Application.CQRS.Commands.Patient;
using CommonLibrary.RabbitMQEvents.PatientEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Patient;

public class PatientCheckConsistancyConsumer : IConsumer<PatientCheckConsistancyEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public PatientCheckConsistancyConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<PatientCheckConsistancyEvent> context)
    {
        var patientCheckConsistancyEvent = context.Message;
        await _mediator.Send(new CheckPatientConsistancyCommand() { PatientCheckConsistancyEvent = patientCheckConsistancyEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
