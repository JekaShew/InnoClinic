using AppointmentAPI.Application.CQRS.Commands.Doctor;
using CommonLibrary.RabbitMQEvents.DoctorEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Doctor;

public class DoctorDeletedConsumer : IConsumer<DoctorDeletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorDeletedConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DoctorDeletedEvent> context)
    {
        var doctorDeletedEvent = context.Message;
        await _mediator.Send(new DeleteDoctorCommand() { Id = doctorDeletedEvent.Id });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
