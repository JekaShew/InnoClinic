using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace AuthorizationAPI.Services.Extensions;

public static class ApplicationServicesExtesionMethods
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationSettings>(configuration.GetSection("Authentication"));
        // Registration of Services
        services.AddScoped<IUserStatusService, UserStatusService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

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
