using AppointmentAPI.Application.CQRS.Commands.Doctor;
using CommonLibrary.RabbitMQEvents.DoctorEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.Doctor;

public class DoctorCheckConsistancyConsumer : IConsumer<DoctorCheckConsistancyEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorCheckConsistancyConsumer(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DoctorCheckConsistancyEvent> context)
    {
        var doctorCheckConsistancyEvent = context.Message;
        await _mediator.Send(new CheckDoctorConsistancyCommand() { DoctorCheckConsistancyEvent = doctorCheckConsistancyEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
