using MassTransit;
using Serilog;

namespace ProfilesAPI.Presentation.RabbitMQConsumers.ConsumerObservers;

public class CustomConsumerObserver : IConsumeObserver
{
    private readonly ILogger _logger;

    public CustomConsumerObserver( ILogger logger)
    {
        _logger = logger;
    }

    public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
    {
        _logger.Information($" Consuming message with Id : {context.MessageId} with Event : {context.Message} started !");
    }

    public Task PostConsume<T>(ConsumeContext<T> context) where T : class
    { 
        return Task.CompletedTask;
    }

    public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
    {
        _logger.Error($" Consuming message with Id : {context.MessageId} with Event : {context.Message} Fault with Error : {exception.Message}!");
    }
}
