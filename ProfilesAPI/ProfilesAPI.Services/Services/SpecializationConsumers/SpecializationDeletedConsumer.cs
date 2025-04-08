using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Services.Services.SpecializationConsumers;

public class SpecializationDeletedConsumer : IConsumer<SpecializationDeletedEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public SpecializationDeletedConsumer(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SpecializationDeletedEvent> context)
    {
        var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(context.Message.Id);

        try
        {
            if(specializationToDelete is not null)
            {
                await _repositoryManager.Specialization.DeleteAsync(specializationToDelete);
                _logger.Information($"Succesfully deleted Specialization with Id: {context.Message.Id}");
            }
            else
            {
                _logger.Information($"Error while deleting Specialization with Id: {context.Message.Id}! No Such Specialization Found!");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Error while deleting Specialization with Id: {context.Message.Id}. Exception: {ex.Message}");
            _logger.Error($"UNABLE to DELETE Specialization with Id:{context.Message.Id}! It's ToDelete Status Changed to TRUE! Please Delete this Specialization with Id: {context.Message.Id} as soon as possible!");
            specializationToDelete.ToDelete = true;
            await _repositoryManager.Specialization.UpdateAsync(specializationToDelete.Id, specializationToDelete);
            _logger.Error($"Error deleting Specialization with Id: {context.Message.Id}. Exception: {ex.Message}");
        }
    }
}
