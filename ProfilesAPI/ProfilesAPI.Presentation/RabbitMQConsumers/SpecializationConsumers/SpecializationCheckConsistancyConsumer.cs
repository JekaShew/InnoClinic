using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationCheckConsistancyConsumer : IConsumer<SpecializationCheckConsistancyEvent>
{
    private readonly ISpecializationService _specializationService;
    private readonly ILogger _logger;

    public SpecializationCheckConsistancyConsumer(ILogger logger, ISpecializationService specializationService)
    {
        _logger = logger;
        _specializationService = specializationService;
    }

    public async Task Consume(ConsumeContext<SpecializationCheckConsistancyEvent> context)
    {
        var specializationCheckConsistancyEvent = context.Message;
        await _specializationService.CheckSpecializationConsistancyAsync(specializationCheckConsistancyEvent);
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
