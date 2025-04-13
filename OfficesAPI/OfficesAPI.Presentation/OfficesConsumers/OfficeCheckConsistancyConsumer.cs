using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using OfficesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace OfficesAPI.Presentation.OfficesConsumers
{
    public class OfficeCheckConsistancyConsumer : IConsumer<OfficeRequestCheckConsistancyEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger _logger;
        private readonly IOfficeService _officeService;

        public OfficeCheckConsistancyConsumer(
            IPublishEndpoint publishEndpoint,
            ILogger logger,
            IOfficeService officeService)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _officeService = officeService;
        }

        public async Task Consume(ConsumeContext<OfficeRequestCheckConsistancyEvent> context)
        {
            _logger.Information($"User with Id: {context.Message.UserId} requested check offices consistancy on ProfilesAPI at {context.Message.DateTime}");
            var consistantOffices = await _officeService.GetAllOfficeCheckConsistancyEventsAsync(); 
            foreach (var consistantOffice in consistantOffices)
            {
                await _publishEndpoint.Publish(consistantOffice);
            }
        }
    }
}
