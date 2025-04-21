using AppointmentAPI.Application.CQRS.Commands.Specialization;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Specialization;

public class SpecializationCreatedConsumer : IConsumer<SpecializationCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public SpecializationCreatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SpecializationCreatedEvent> context)
    {
        var specializationCreatedEvent = context.Message;
        await _mediator.Send(new CreateSpecializationCommand() { SpecializationCreatedEvent = specializationCreatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
