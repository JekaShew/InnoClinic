using System.Reflection;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesAPI.Application.RabbitMQConsumers.SpecializationConsumers;

namespace ServicesAPI.Application.Extensions;

public static class ServiceExntensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapperService();
        services.AddMediatorService();
        services.AddFluentValidationService();
        services.AddRabbitMQService(configuration);

        return services;
    }

    public static IServiceCollection AddMediatorService(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(CQRS.Commands.ServiceCommands.CreateServiceCommand).Assembly);
        });

        return services;
    }
    public static IServiceCollection AddFluentValidationService(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ServicesAPI.Application.Validators.ServiceValidators.ServiceForCreateDTOValidator).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    private static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration configuration)
    {
        // RabbitMQ + MassTransit
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<SpecializationRequestCheckConsistancyConsumer>();

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

    private static IServiceCollection AddAutoMapperService(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
