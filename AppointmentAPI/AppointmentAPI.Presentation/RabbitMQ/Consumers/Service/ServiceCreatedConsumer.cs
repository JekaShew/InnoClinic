using AppointmentAPI.Application.CQRS.Commands.Service;
using CommonLibrary.RabbitMQEvents.ServiceEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Service;

public class ServiceCreatedConsumer : IConsumer<ServiceCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ServiceCreatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ServiceCreatedEvent> context)
    {
        var serviceCreatedEvent = context.Message;
        await _mediator.Send(new CreateServiceCommand() { ServiceCreatedEvent = serviceCreatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
