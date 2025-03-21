using AutoMapper;
using CommonLibrary.RabbitMQEvents;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;

namespace ProfilesAPI.Services.Services.OfficesConsumers;

public class OfficeUpdatedConsumer : IConsumer<OfficeUpdatedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    public OfficeUpdatedConsumer(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<OfficeUpdatedEvent> context)
    {
        var office = _mapper.Map<Office>(context.Message);
        await _repositoryManager.Office.UpdateAsync(context.Message.Id, office);
    }
}
