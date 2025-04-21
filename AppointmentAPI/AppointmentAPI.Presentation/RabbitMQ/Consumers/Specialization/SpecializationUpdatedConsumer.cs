using AppointmentAPI.Application.CQRS.Commands.Specialization;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Specialization;

public class SpecializationUpdatedConsumer : IConsumer<SpecializationUpdatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public SpecializationUpdatedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<SpecializationUpdatedEvent> context)
    {
        var specializationUpdatedEvent = context.Message;
        await _mediator.Send(new UpdateSpecializationCommand() { SpecializationUpdatedEvent = specializationUpdatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
