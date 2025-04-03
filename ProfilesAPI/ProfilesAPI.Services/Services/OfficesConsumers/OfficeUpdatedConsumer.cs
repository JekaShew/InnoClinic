using AutoMapper;
using CommonLibrary.RabbitMQEvents;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Services.Services.OfficesConsumers;

public class OfficeUpdatedConsumer : IConsumer<OfficeUpdatedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    public OfficeUpdatedConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<OfficeUpdatedEvent> context)
    {
        var office = _mapper.Map<Office>(context.Message);
        await _repositoryManager.Office.UpdateAsync(context.Message.Id, office);
        _logger.Information($"Succesfully updated Office: {office}");
    }
}
