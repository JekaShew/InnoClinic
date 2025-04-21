using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.DoctorSpecialization;

public class DoctorSpecializationCreatedConsumer : IConsumer<DoctorSpecializationCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorSpecializationCreatedConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DoctorSpecializationCreatedEvent> context)
    {
        var doctorSpecializationCreatedEvent = context.Message;
        await _mediator.Send(new CreateDoctorSpecializationCommand() { DoctorSpecializationCreatedEvent = doctorSpecializationCreatedEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
