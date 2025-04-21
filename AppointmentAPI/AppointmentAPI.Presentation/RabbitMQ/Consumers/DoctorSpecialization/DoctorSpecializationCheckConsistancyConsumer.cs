using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Presentation.RabbitMQ.Consumers.DoctorSpecialization;

public class DoctorSpecializationCheckConsistancyConsumer : IConsumer<DoctorSpecializationCheckConsistancyEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public DoctorSpecializationCheckConsistancyConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DoctorSpecializationCheckConsistancyEvent> context)
    {
        var doctorSpecializationCheckConsistancyEvent = context.Message;
        await _mediator.Send(new CheckDoctorSpecializationConsistancyCommand() { DoctorSpecializationCheckConsistancyEvent = doctorSpecializationCheckConsistancyEvent });
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
