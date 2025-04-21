using AppointmentAPI.Application.CQRS.Commands.Office;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Office;

public class OfficeUpdatedConsumer : IConsumer<OfficeUpdatedEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public OfficeUpdatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OfficeUpdatedEvent> context)
    {
        var officeUpdatedEvent = context.Message;
        await _mediator.Send(new UpdateOfficeCommand() { OfficeUpdatedEvent = officeUpdatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
