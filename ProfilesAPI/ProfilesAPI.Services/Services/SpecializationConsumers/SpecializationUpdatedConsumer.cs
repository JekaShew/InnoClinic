using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationUpdatedConsumer : IConsumer<SpecializationUpdatedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SpecializationUpdatedConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SpecializationUpdatedEvent> context)
    {
        var specialization = _mapper.Map<Specialization>(context.Message);
        await _repositoryManager.Specialization.UpdateAsync(context.Message.Id, specialization);
        _logger.Information($"Succesfully updated Specialization: {specialization}");
    }
}
