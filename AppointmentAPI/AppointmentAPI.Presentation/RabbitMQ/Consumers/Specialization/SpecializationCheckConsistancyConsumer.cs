using AppointmentAPI.Application.CQRS.Commands.Specialization;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Specialization;

public class SpecializationCheckConsistancyConsumer : IConsumer<SpecializationCheckConsistancyEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public SpecializationCheckConsistancyConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SpecializationCheckConsistancyEvent> context)
    {
        var specializationCheckConsistancyEvent = context.Message;
        await _mediator.Send(new CheckSpecializationConsistancyCommand() { SpecializationCheckConsistancyEvent = specializationCheckConsistancyEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
