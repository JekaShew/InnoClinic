using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationCreatedConsumer : IConsumer<SpecializationCreatedEvent>
{
    private readonly ISpecializationService _specializationService;
    private readonly ILogger _logger;

    public SpecializationCreatedConsumer( ILogger logger, ISpecializationService specializationService)
    {
        _logger = logger;
        _specializationService = specializationService;
    }

    public async Task Consume(ConsumeContext<SpecializationCreatedEvent> context)
    {
        var specializationCreatedEvent = context.Message;
        await _specializationService.CreateSpecializationAsync(specializationCreatedEvent); 
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }
}
