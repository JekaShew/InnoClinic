using AppointmentAPI.Application.CQRS.Commands.Office;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Office;

public class OfficeCheckConsistancyConsumer : IConsumer<OfficeCheckConsistancyEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public OfficeCheckConsistancyConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OfficeCheckConsistancyEvent> context)
    {
        var officeCheckConsistancyEvent = context.Message;
        await _mediator.Send(new CheckOfficeConsistancyCommand() { OfficeCheckConsistancyEvent = officeCheckConsistancyEvent});
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
