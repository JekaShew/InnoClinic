using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationCreatedConsumer : IConsumer<SpecializationCreatedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SpecializationCreatedConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SpecializationCreatedEvent> context)
    {
        var specialziation = _mapper.Map<Specialization>(context.Message);
        await _repositoryManager.Specialization.CreateAsync(specialziation);
        _logger.Information($"Succesfully added Specialization: {specialziation}");
    }
}
