using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.DoctorSpecialization;

public class DoctorSpecializationUpdatedConsumer : IConsumer<DoctorSpecializationUpdatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorSpecializationUpdatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DoctorSpecializationUpdatedEvent> context)
    {
        var doctorSpecializationUpdatedEvent = context.Message;
        await _mediator.Send(new UpdateDoctorSpecializationCommand() { DoctorSpecializationUpdatedEvent = doctorSpecializationUpdatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
