using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.OfficeConsumers;

public class OfficeCheckConsistancyConsumer : IConsumer<OfficeCheckConsistancyEvent>
{
    private readonly ILogger _logger;
    private readonly IOfficeService _officeService;

    public OfficeCheckConsistancyConsumer(ILogger logger, IOfficeService officeService)
    {
        _logger = logger;
        _officeService = officeService;
    }
    public async Task Consume(ConsumeContext<OfficeCheckConsistancyEvent> context)
    {
        var officeConsistancyEvent = context.Message;
        await _officeService.CheckOfficeConsistancyAsync(officeConsistancyEvent);
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
