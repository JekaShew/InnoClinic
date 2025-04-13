using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.ConsumerObservers.OfficeConsumerObservers;

public class OfficeDeletedConsumerObserver : IConsumeMessageObserver<OfficeDeletedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public OfficeDeletedConsumerObserver(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task PreConsume(ConsumeContext<OfficeDeletedEvent> context)
    {
        var officeToDelete = await _repositoryManager.Office.GetByIdAsync(context.Message.Id);
        if (officeToDelete is null)
        {
            _logger.Information($"Error while deleting Office with Id: {context.Message.Id}! No Such Office Found!");
        }
    }

    public Task PostConsume(ConsumeContext<OfficeDeletedEvent> context)
    {
        return Task.CompletedTask;
    }

    public async Task ConsumeFault(ConsumeContext<OfficeDeletedEvent> context, Exception exception)
    {
        var officeToDelete = await _repositoryManager.Office.GetByIdAsync(context.Message.Id);
        _logger.Error($"Error deleting Office with Id: {context.Message.Id}. Exception: {exception.Message}");
        _logger.Error($"UNABLE to DELETE Office with Id:{context.Message.Id}! It's IsDelete Status Changed to TRUE! Please Delete this Office with Id: {context.Message.Id} as soon as possible!");
        officeToDelete.IsDelete = true;
        await _repositoryManager.Office.UpdateAsync(officeToDelete.Id, officeToDelete);
        _logger.Error($"Error deleting Office with Id: {context.Message.Id}. Exception: {exception.Message}");
    }
}
