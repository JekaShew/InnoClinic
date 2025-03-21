using CommonLibrary.RabbitMQEvents;
using MassTransit;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Shared.Mappers;

namespace OfficesAPI.Services.Services.OfficesConsumers
{
    public class OfficeCheckConsistancyConsumer : IConsumer<OfficeRequestCheckConsistancyEvent>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IPublishEndpoint _publishEndpoint;
        public OfficeCheckConsistancyConsumer(IRepositoryManager repositoryManager, IPublishEndpoint publishEndpoint)
        {
            _repositoryManager = repositoryManager;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OfficeRequestCheckConsistancyEvent> context)
        {
            // log event userId requested check at dateTime
            var consistantOffices = await _repositoryManager.Office.GetAllOfficesAsync();
            foreach (var consistantOffice in consistantOffices)
            {
                var officeCheckConsistancyEvent = OfficeMapper.OfficeToOfficeCheckConsistancyEvent(consistantOffice);
                await _publishEndpoint.Publish(officeCheckConsistancyEvent);
            }
        }
    }
}
