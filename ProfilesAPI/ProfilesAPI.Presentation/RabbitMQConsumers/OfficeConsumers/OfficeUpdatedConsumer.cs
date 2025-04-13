using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.OfficeConsumers;

public class OfficeUpdatedConsumer : IConsumer<OfficeUpdatedEvent>
{
    private readonly ILogger _logger;
    private readonly IOfficeService _officeService;

    public OfficeUpdatedConsumer(ILogger logger, IOfficeService officeService)
    {
        _logger = logger;
        _officeService = officeService;
    }
    public async Task Consume(ConsumeContext<OfficeUpdatedEvent> context)
    {
        var officeUpdatedEvent = context.Message;
        await _officeService.UpdateOfficeAsync(officeUpdatedEvent);
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
