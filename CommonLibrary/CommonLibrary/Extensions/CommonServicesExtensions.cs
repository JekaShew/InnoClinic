using CommonLibrary.CommonService;
using InnoClinic.CommonLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace InnoClinic.CommonLibrary.Exceptions;

public static class CommonServicesExtensions
{

    public static IHostBuilder AddSerilogMethod(this IHostBuilder builder, IConfiguration configuration, string serilogFile)
    {
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();

        builder.UseSerilog((context, serilogConfiguration) =>
        {
            serilogConfiguration
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithThreadId()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["Elasticsearch:ServerUrlDocker"]))
                {
                    MinimumLogEventLevel = LogEventLevel.Information,
                    IndexFormat = $"{context.Configuration["ApplicationName"]}--logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}",
                    AutoRegisterTemplate = true,
                    InlineFields = true,
                    NumberOfReplicas = 2,
                    NumberOfShards = 2
                })
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", configuration["ApplicationName"])
                .Enrich.WithThreadId()
                .WriteTo.Seq(configuration["Seq:ServerUrlDocker"])
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
                 .Enrich.WithProperty("ApplicationName", configuration["ApplicationName"])
                .Enrich.WithThreadId()
                .WriteTo.File($"{serilogFile}.log");
        });

             return builder;
    }
    public static IServiceCollection AddCommonServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {       
        // CommonService
        services.AddScoped<ICommonService, CommonService>();

        // Global Exception Handler
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // JWT Authentication Scheme
        JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, configuration);

        // Redis Cache
        services.AddRedisCache(configuration);

        return services;
    }

    public static IServiceCollection AddRedisCache(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisDocker");
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }

    public static IApplicationBuilder UseCommonPolicies(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(opt => { });
        app.UseSerilogRequestLogging();

        return app;
    }
}
