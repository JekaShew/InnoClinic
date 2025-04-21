using AppointmentAPI.Application.CQRS.Commands.Patient;
using CommonLibrary.RabbitMQEvents.PatientEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Patient;

public class PatientDeletedConsumer : IConsumer<PatientDeletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public PatientDeletedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PatientDeletedEvent> context)
    {
        var patientDeletedEvent = context.Message;
        await _mediator.Send(new DeletePatientCommand() { Id = patientDeletedEvent.Id });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
