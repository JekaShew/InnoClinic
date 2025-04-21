using AppointmentAPI.Application.CQRS.Commands.Office;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Office;

public class OfficeCreatedConsumer : IConsumer<OfficeCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public OfficeCreatedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OfficeCreatedEvent> context)
    {
        var officeCreatedEvent = context.Message;
        await _mediator.Send(new CreateOfficeCommand() { OfficeCreatedEvent = officeCreatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
