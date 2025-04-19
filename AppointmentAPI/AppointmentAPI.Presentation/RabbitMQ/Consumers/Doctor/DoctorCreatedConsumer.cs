using AppointmentAPI.Application.CQRS.Commands.Doctor;
using CommonLibrary.RabbitMQEvents.DoctorEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Doctor;

public class DoctorCreatedConsumer : IConsumer<DoctorCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public DoctorCreatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DoctorCreatedEvent> context)
    {
        var doctorCreatedEvent = context.Message;
        await _mediator.Send(new CreateDoctorCommand() { DoctorCreatedEvent = doctorCreatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
