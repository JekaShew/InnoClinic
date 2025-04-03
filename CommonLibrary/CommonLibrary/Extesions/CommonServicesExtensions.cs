using CommonLibrary.CommonService;
using InnoClinic.CommonLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace InnoClinic.CommonLibrary.Exceptions;

public static class CommonServicesExtensions
{
    public static IServiceCollection AddCommonServices(
        this IServiceCollection services,
        IConfiguration configuration,
        string serilogFile)
    {       
        // CommonService
        services.AddScoped<ICommonService, CommonService>();
        //Serilog
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.File($"{serilogFile}.log")
        .CreateBootstrapLogger();

        services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(LogEventLevel.Error)
            .WriteTo.File($"{serilogFile}.log"));

        // Global Exception Handler
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // JWT Authentication Scheme
        JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, configuration);

        return services;
    }
    public static IApplicationBuilder UseCommonPolicies(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(opt => { });

        return app;
    }
}
