using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Services;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace AuthorizationAPI.Services.Extensions;

public static class ApplicationServicesExtesionMethods
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AuthorizationJWTSettings>()
            .Bind(configuration.GetSection(AuthorizationJWTSettings.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<FromEmailsSettings>()
            .Bind(configuration.GetSection(FromEmailsSettings.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Registration of Services
        services.AddScoped<IUserStatusService, UserStatusService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        services.AddScoped<IEmailService, FluentEmailService>();

        services.AddFluentValidationMethod();
        services.AddFluentEmailMethod(configuration);
        services.AddMemoryCache(opt =>
        {
            opt.SizeLimit = 1000;
        });
        services.AddHangFireMethod(configuration);
        
        return services;
    }

    private static IServiceCollection AddFluentEmailMethod(this IServiceCollection services, IConfiguration configuration)
    {
        // FluentEmail
        var emailSettings = configuration.GetSection("EmailSettings");
        var defaultFromEmail = emailSettings["FromEmails:Default"];
        var host = emailSettings["SMTPSettings:Host"];
        var port = emailSettings.GetValue<int>("SMTPSettings:Port");

        services.AddFluentEmail(defaultFromEmail)
           .AddSmtpSender(host, port);


        return services;
    }

    private static IServiceCollection AddFluentValidationMethod(this IServiceCollection services)
    {
        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddHangFireMethod(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BackgroundTasks>();
        services.AddHangfire(config =>
            config.UseSimpleAssemblyNameTypeSerializer()
            .UseSimpleAssemblyNameTypeSerializer()
            .UseSqlServerStorage(configuration.GetConnectionString("AuthDB"))
        );

        services.AddHangfireServer();


        return services;
    }
    public static IApplicationBuilder StartBackgroundTasks(this IApplicationBuilder app)
    {
        IRecurringJobManager recurringJobManager = new RecurringJobManager();
        recurringJobManager.AddOrUpdate<BackgroundTasks>(
                "CleaningExpiredRefresTokens",
                x => x.CleanExpiredRefreshTokensAsync(),
                "*/15 * * * *");  // Crone = Every 15 Minutes 

        return app;      
    }
}
