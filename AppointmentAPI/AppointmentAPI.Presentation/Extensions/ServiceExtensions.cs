using AppointmentAPI.Presentation.RabbitMQ;
using AppointmentAPI.Presentation.RabbitMQ.Consumers.Doctor;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentAPI.Presentation.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQService(configuration);

        return services;
    }

    private static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration configuration)
    {

        // RabbitMQ + MassTransit
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            //Add consumers 
            busConfigurator.AddConsumersFromNamespaceContaining<DoctorCreatedConsumer>();
            busConfigurator.AddConsumeObserver<CustomConsumerObserver>();
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration["MessageBroker:HostDocker"], configuration["MessageBroker:Port"], configuration["MessageBroker:VirtualHost"], hostConfigurator =>
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
