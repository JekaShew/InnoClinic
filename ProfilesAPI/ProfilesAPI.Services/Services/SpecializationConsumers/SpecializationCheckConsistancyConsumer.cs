using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationCheckConsistancyConsumer : IConsumer<SpecializationCheckConsistancyEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SpecializationCheckConsistancyConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SpecializationCheckConsistancyEvent> context)
    {
        var specialization = await _repositoryManager.Specialization.GetByIdAsync(context.Message.Id);
        var consistantSpecialization = _mapper.Map<Specialization>(context.Message);
        if (specialization is null)
        {
            await _repositoryManager.Specialization.CreateAsync(consistantSpecialization);
            _logger.Information($"Succesfully added Specialization: {consistantSpecialization}");
        }

        if (specialization is not null && !specialization.Equals(consistantSpecialization))
        {
            await _repositoryManager.Specialization.UpdateAsync(consistantSpecialization.Id, consistantSpecialization);
            _logger.Information($"Succesfully updated Specialization: {consistantSpecialization}");
        }
    }
}
