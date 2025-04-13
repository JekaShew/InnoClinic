using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.OfficeConsumers;

public class OfficeCreatedConsumer : IConsumer<OfficeCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly IOfficeService _officeService;

    public OfficeCreatedConsumer(ILogger logger, IOfficeService officeService)
    {
        _logger = logger;
        _officeService = officeService;
    }
    public async Task Consume(ConsumeContext<OfficeCreatedEvent> context)
    {
        var officeCreatedEvent = context.Message;
        await _officeService.CreateOfficeAsync(officeCreatedEvent);
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
