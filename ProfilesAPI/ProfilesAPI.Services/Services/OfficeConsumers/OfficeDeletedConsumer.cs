using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Domain.IRepositories;
using Serilog;

namespace ProfilesAPI.Services.Services.OfficeConsumers
{
    public class OfficeDeletedConsumer : IConsumer<OfficeDeletedEvent>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger _logger;

        public OfficeDeletedConsumer(
                IRepositoryManager repositoryManager,
                ILogger logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OfficeDeletedEvent> context)
        {
            var officeToDelete = await _repositoryManager.Office.GetByIdAsync(context.Message.Id);
            try
            {
                if (officeToDelete is not null)
                {
                    await _repositoryManager.Office.DeleteAsync(officeToDelete);
                    _logger.Information($"Succesfully deleted Office with Id: {context.Message.Id}");
                }
                else
                {
                    _logger.Information($"Error while deleting Office with Id: {context.Message.Id}! No Such Office Found!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting Office with Id: {context.Message.Id}. Exception: {ex.Message}");
                _logger.Error($"UNABLE to DELETE Office with Id:{context.Message.Id}! It's ToDelete Status Changed to TRUE! Please Delete this Office with Id: {context.Message.Id} as soon as possible!");
                officeToDelete.ToDelete = true;
                await _repositoryManager.Office.UpdateAsync(officeToDelete.Id, officeToDelete);
                _logger.Error($"Error deleting Office with Id: {context.Message.Id}. Exception: {ex.Message}");
            }
        }
    }
}
