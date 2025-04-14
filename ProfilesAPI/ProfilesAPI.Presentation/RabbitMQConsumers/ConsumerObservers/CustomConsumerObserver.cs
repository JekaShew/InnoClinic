using CommonLibrary.RabbitMQEvents.OfficeEvents;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.ConsumerObservers;

public class CustomConsumerObserver : IConsumeObserver
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public CustomConsumerObserver(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
    {
        switch (context.Message)
        {
            case OfficeDeletedEvent officeDeletedEvent:
                var officeToDelete = await _repositoryManager.Office.GetByIdAsync(officeDeletedEvent.Id);
                if (officeToDelete is null)
                {
                    _logger.Information($"Error while deleting Office with Id: {officeDeletedEvent.Id}! No Such Office Found!");
                }
                break;

            case SpecializationDeletedEvent specializationDeletedEvent:
                var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(specializationDeletedEvent.Id);
                if (specializationToDelete is null)
                {
                    _logger.Information($"Error while deleting Specialization with Id: {specializationDeletedEvent.Id}! No Such Specialization Found!");
                }
                break;

            default:
                var type = context.Message.GetType();
                _logger.Information($"Some Event with type : {type.Name} started consuming!");
                break;
        }        
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

    public Task PostConsume<T>(ConsumeContext<T> context) where T : class
    {
        return Task.CompletedTask;
    }

    public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
    {
        switch (context.Message)
        {
            case OfficeDeletedEvent officeDeletedEvent:
                var officeToDelete = await _repositoryManager.Office.GetByIdAsync(officeDeletedEvent.Id);
                _logger.Error($"Error deleting Office with Id: {officeDeletedEvent.Id}. Exception: {exception.Message}");
                _logger.Error($"UNABLE to DELETE Office with Id:{officeDeletedEvent.Id}! It's IsDelete Status Changed to TRUE! Please Delete this Office with Id: {officeDeletedEvent.Id} as soon as possible!");
                officeToDelete.IsDelete = true;
                await _repositoryManager.Office.UpdateAsync(officeToDelete.Id, officeToDelete);
                _logger.Error($"Error deleting Office with Id: {officeDeletedEvent.Id}. Exception: {exception.Message}");
                break;

            case SpecializationDeletedEvent specializationDeletedEvent:
                var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(specializationDeletedEvent.Id);

                _logger.Error($"Error while deleting Specialization with Id: {specializationDeletedEvent.Id}. Exception: {exception.Message}");
                _logger.Error($"UNABLE to DELETE Specialization with Id:{specializationDeletedEvent.Id}! It's IsDelete Status Changed to TRUE! Please Delete this Specialization with Id: {specializationDeletedEvent.Id} as soon as possible!");
                specializationToDelete.IsDelete = true;
                await _repositoryManager.Specialization.UpdateAsync(specializationToDelete.Id, specializationToDelete);
                _logger.Error($"Error deleting Specialization with Id: {specializationDeletedEvent.Id}. Exception: {exception.Message}");
                break;

            default:
                var type = context.Message.GetType();
                _logger.Error($"Some Event with type : {type.Name} Fault with Error : {exception.Message}!");
                break;
        }
    }
}
