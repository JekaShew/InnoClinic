using CommonLibrary.RabbitMQEvents;
using MassTransit;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Shared.Mappers;
using Serilog;

namespace OfficesAPI.Services.Services.OfficesConsumers
{
    public class OfficeCheckConsistancyConsumer : IConsumer<OfficeRequestCheckConsistancyEvent>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger _logger;
        public OfficeCheckConsistancyConsumer(
            IRepositoryManager repositoryManager,
            IPublishEndpoint publishEndpoint,
            ILogger logger)
        {
            _repositoryManager = repositoryManager;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OfficeRequestCheckConsistancyEvent> context)
        {
            _logger.Information($"User with Id: {context.Message.UserId} requested check offices consistancy on ProfilesAPI at {context.Message.DateTime}");
            var consistantOffices = await _repositoryManager.Office.GetAllOfficesAsync();
            foreach (var consistantOffice in consistantOffices)
            {
                var officeCheckConsistancyEvent = OfficeMapper.OfficeToOfficeCheckConsistancyEvent(consistantOffice);
                await _publishEndpoint.Publish(officeCheckConsistancyEvent);
            }
        }
    }
}
