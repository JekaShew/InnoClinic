using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.DoctorSpecialization;

public class DoctorSpecializationDeletedConsumer : IConsumer<DoctorSpecializationDeletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorSpecializationDeletedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DoctorSpecializationDeletedEvent> context)
    {
        var doctorSpecializationDeletedEvent = context.Message;
        await _mediator.Send(new DeleteDoctorSpecializationCommand() { Id = doctorSpecializationDeletedEvent.Id });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
