using AppointmentAPI.Application.CQRS.Commands.Doctor;
using CommonLibrary.RabbitMQEvents.DoctorEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Doctor;

public class DoctorUpdatedConsumer : IConsumer<DoctorUpdatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorUpdatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DoctorUpdatedEvent> context)
    {
        var doctorUpdatedEvent = context.Message;
        await _mediator.Send(new UpdateDoctorCommand() { DoctorUpdatedEvent = doctorUpdatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
