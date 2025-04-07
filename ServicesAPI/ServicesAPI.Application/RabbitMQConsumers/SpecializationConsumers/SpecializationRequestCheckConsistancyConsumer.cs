using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using Serilog;
using ServicesAPI.Domain.Data.IRepositories;

namespace ServicesAPI.Application.RabbitMQConsumers.SpecializationConsumers;

public class SpecializationRequestCheckConsistancyConsumer : IConsumer<SpecializationRequestCheckConsistancyEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public SpecializationRequestCheckConsistancyConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger, IPublishEndpoint publishEndpoint)
    {
        _repositoryManager = repositoryManager;

        _mapper = mapper;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Consume(ConsumeContext<SpecializationRequestCheckConsistancyEvent> context)
    {
        _logger.Information($"User with Id: {context.Message.UserId} requested check specializations consistancy on ProfilesAPI at {context.Message.DateTime}");
        var consistantSpecializations = await _repositoryManager.Specialization.GetAllAsync();
        foreach (var consistantSpecialization in consistantSpecializations)
        {
            var specializationCheckConsistancyEvent = _mapper.Map<SpecializationCheckConsistancyEvent>(consistantSpecialization);
            await _publishEndpoint.Publish(specializationCheckConsistancyEvent);
        }
    }
}
