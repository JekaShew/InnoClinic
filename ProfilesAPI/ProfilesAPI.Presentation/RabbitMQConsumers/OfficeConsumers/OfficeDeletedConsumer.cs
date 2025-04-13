using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.OfficeConsumers
{
    public class OfficeDeletedConsumer : IConsumer<OfficeDeletedEvent>
    {
        private readonly IOfficeService _officeService;
        private readonly ILogger _logger;

        public OfficeDeletedConsumer(
                ILogger logger,
                IOfficeService officeService)
        {
            _logger = logger;
            _officeService = officeService;
        }

        public async Task Consume(ConsumeContext<OfficeDeletedEvent> context)
        {
            var officeDeletedEvent = context.Message;
            await _officeService.DeleteOfficeAsync(officeDeletedEvent);
            _logger.Information($"Succesfully consumed message with ID : {context.MessageId} and Message : {context.Message}!");
        }
    }
}
