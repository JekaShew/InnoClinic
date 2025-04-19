using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppointmentAPI.Application.Extensions;

public static class ServiceExtensions
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
        // MediatR
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(CQRS.Commands.Appointment.CreateAppointmentCommand).Assembly);
        });

        return services;
    }

    public static IServiceCollection AddFluentValidationService(this IServiceCollection services)
    {
        // FluentValidation in MediatR
        services.AddValidatorsFromAssembly(typeof(AppointmentAPI.Application.Validators.AppointmentValidators.AppointmentForCreateDTOValidator).Assembly);
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
