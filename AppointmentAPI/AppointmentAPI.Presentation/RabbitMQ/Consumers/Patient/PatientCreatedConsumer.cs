using AppointmentAPI.Application.CQRS.Commands.Patient;
using CommonLibrary.RabbitMQEvents.PatientEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Patient;

public class PatientCreatedConsumer : IConsumer<PatientCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public PatientCreatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PatientCreatedEvent> context)
    {
        var patientCreatedEvent = context.Message;
        await _mediator.Send(new CreatePatientCommand() { PatientCreatedEvent = patientCreatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
