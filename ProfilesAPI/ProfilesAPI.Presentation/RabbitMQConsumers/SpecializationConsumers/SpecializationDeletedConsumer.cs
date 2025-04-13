using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationDeletedConsumer : IConsumer<SpecializationDeletedEvent>
{
    private readonly ISpecializationService _specializationService;
    private readonly ILogger _logger;

    public SpecializationDeletedConsumer(ILogger logger, ISpecializationService specializationService)
    {        
        _logger = logger;
        _specializationService = specializationService;
    }

    public async Task Consume(ConsumeContext<SpecializationDeletedEvent> context)
    {
        var specializationDeletedEvent = context.Message;
        await _specializationService.DeleteSpecializationAsync(specializationDeletedEvent);
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
