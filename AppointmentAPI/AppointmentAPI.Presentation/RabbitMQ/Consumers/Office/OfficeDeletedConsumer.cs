using AppointmentAPI.Application.CQRS.Commands.Office;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Officel;

public class OfficeDeletedConsumer : IConsumer<OfficeDeletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public OfficeDeletedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OfficeDeletedEvent> context)
    {
        var officeDeletedEvent = context.Message;
        await _mediator.Send(new DeleteOfficeCommand() { Id = officeDeletedEvent.Id });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
