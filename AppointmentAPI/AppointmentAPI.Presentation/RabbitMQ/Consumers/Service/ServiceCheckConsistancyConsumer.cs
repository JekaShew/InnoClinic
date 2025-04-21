using AppointmentAPI.Application.CQRS.Commands.Service;
using CommonLibrary.RabbitMQEvents.ServiceEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Service;

public class ServiceCheckConsistancyConsumer : IConsumer<ServiceCheckConsistancyEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ServiceCheckConsistancyConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ServiceCheckConsistancyEvent> context)
    {
        var serviceCheckСonsistancyEvent = context.Message;
        await _mediator.Send(new CheckServiceConsistancyCommand() { ServiceCheckConsistancyEvent = serviceCheckСonsistancyEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
