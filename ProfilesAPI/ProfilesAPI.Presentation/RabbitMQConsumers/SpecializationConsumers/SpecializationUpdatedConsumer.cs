using CommonLibrary.RabbitMQEvents.OfficeEvents;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationUpdatedConsumer : IConsumer<SpecializationUpdatedEvent>
{
    private readonly ISpecializationService _specializationService;
    private readonly ILogger _logger;

    public SpecializationUpdatedConsumer(
            ILogger logger, 
            ISpecializationService specializationService)
    {
        _logger = logger;
        _specializationService = specializationService;
    }

    public async Task Consume(ConsumeContext<SpecializationUpdatedEvent> context)
    {
        var specializationUpdatedEvent = context.Message;
        await _specializationService.UpdateSpecializationAsync(specializationUpdatedEvent);
        _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
    }

    
}
