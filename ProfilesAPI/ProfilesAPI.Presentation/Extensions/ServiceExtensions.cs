using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProfilesAPI.Presentation.RabbitMQConsumers.ConsumerObservers.OfficeConsumerObservers;
using ProfilesAPI.Presentation.RabbitMQConsumers.ConsumerObservers.SpecializationConsumerObservers;
using ProfilesAPI.Presentation.RabbitMQConsumers.OfficeConsumers;
using ProfilesAPI.Services.Services.SpecializationConsumers;

namespace ProfilesAPI.Presentation.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQService(configuration);

        return services;
    }

    private static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddConsumeObserver<OfficeDeletedConsumerObserver>();
        //services.AddConsumeObserver<SpecializationDeletedConsumerObserver>();
        //services.AddConsumeObserver(provider => new OfficeDeletedConsumerObserver());
        //services.AddConsumeObserver(provider => new SpecializationDeletedConsumerObserver());


        // RabbitMQ + MassTransit
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumersFromNamespaceContaining<SpecializationCreatedConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration["MessageBroker:HostDocker"], 5672, "/", hostConfigurator =>
                {
                    hostConfigurator.Username(configuration["MessageBroker:Username"]);
                    hostConfigurator.Password(configuration["MessageBroker:Password"]);
                });


                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
