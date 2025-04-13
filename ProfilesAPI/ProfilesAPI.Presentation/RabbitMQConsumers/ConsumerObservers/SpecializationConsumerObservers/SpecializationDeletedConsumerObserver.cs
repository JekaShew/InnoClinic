using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.ConsumerObservers.SpecializationConsumerObservers;

public class SpecializationDeletedConsumerObserver : IConsumeMessageObserver<SpecializationDeletedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public SpecializationDeletedConsumerObserver(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task PreConsume(ConsumeContext<SpecializationDeletedEvent> context)
    {
        var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(context.Message.Id);
        if (specializationToDelete is null)
        {
            _logger.Information($"Error while deleting Specialization with Id: {context.Message.Id}! No Such Specialization Found!");
        }
    }

    public Task PostConsume(ConsumeContext<SpecializationDeletedEvent> context)
    {
        return Task.CompletedTask;
    }

    public async Task ConsumeFault(ConsumeContext<SpecializationDeletedEvent> context, Exception exception)
    {
        var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(context.Message.Id);

        _logger.Error($"Error while deleting Specialization with Id: {context.Message.Id}. Exception: {exception.Message}");
        _logger.Error($"UNABLE to DELETE Specialization with Id:{context.Message.Id}! It's IsDelete Status Changed to TRUE! Please Delete this Specialization with Id: {context.Message.Id} as soon as possible!");
        specializationToDelete.IsDelete = true;
        await _repositoryManager.Specialization.UpdateAsync(specializationToDelete.Id, specializationToDelete);
        _logger.Error($"Error deleting Specialization with Id: {context.Message.Id}. Exception: {exception.Message}");
    }
}
