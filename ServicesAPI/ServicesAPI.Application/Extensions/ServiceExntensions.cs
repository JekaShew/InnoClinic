using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServicesAPI.Application.Extensions;

public static class ServiceExntensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapperService();
        services.AddMediatorService();
        services.AddFluentValidationService();

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

    private static IServiceCollection AddAutoMapperService(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
