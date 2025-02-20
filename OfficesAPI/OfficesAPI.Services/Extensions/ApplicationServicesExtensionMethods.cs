using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Services.Services;
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


        return services;
    }

    private static IServiceCollection AddFluentValidationMethod(this IServiceCollection services)
    {
        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
