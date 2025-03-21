using CommonLibrary.RabbitMQEvents;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Services.Services;
using OfficesAPI.Services.Services.OfficesConsumers;
using System.Reflection;

namespace OfficesAPI.Services.Extensions;

public static class ApplicationServicesExtensionMethods
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Registration of Services
        services.AddScoped<IOfficeService, OfficeService>();
        services.AddScoped<IPhotoService, PhotoService>();

        services.AddFluentValidationMethod();
        services.AddRabbitMQMethod(configuration);

        return services;
    }

    private static IServiceCollection AddFluentValidationMethod(this IServiceCollection services)
    {
        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddRabbitMQMethod(this IServiceCollection services, IConfiguration configuration)
    {
        // FluentValidation
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<OfficeCheckConsistancyConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]), hostConfigurator =>
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
