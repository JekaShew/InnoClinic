using AppointmentAPI.Application.CQRS.Commands.Service;
using CommonLibrary.RabbitMQEvents.ServiceEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Service;

public class ServiceUpdatedConsumer : IConsumer<ServiceUpdatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ServiceUpdatedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ServiceUpdatedEvent> context)
    {
        var serviceUpdatedEvent = context.Message;
        await _mediator.Send(new UpdateServiceCommand() { ServiceUpdatedEvent = serviceUpdatedEvent});
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
