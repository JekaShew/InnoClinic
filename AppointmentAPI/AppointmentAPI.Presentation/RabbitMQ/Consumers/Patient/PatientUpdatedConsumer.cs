using AppointmentAPI.Application.CQRS.Commands.Patient;
using CommonLibrary.RabbitMQEvents.PatientEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Patient;

public class PatientUpdatedConsumer : IConsumer<PatientUpdatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public PatientUpdatedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<PatientUpdatedEvent> context)
    {
        var patientUpdatedEvent = context.Message;
        await _mediator.Send(new UpdatePatientCommand() { PatientUpdatedEvent = patientUpdatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
