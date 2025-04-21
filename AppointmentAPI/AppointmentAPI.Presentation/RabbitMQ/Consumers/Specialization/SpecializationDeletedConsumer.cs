using AppointmentAPI.Application.CQRS.Commands.Specialization;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Specialization;

public class SpecializationDeletedConsumer : IConsumer<SpecializationDeletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public SpecializationDeletedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<SpecializationDeletedEvent> context)
    {
        var specializationDeletedEvent = context.Message;
        await _mediator.Send(new DeleteSpecializationCommand() { Id = specializationDeletedEvent.Id });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
