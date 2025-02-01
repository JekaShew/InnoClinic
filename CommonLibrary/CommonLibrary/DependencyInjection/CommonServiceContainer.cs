using InnoClinic.CommonLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace InnoClinic.CommonLibrary.DependencyInjection
{
    public static class CommonServiceContainer
    {
         
        public static IServiceCollection AddCommonServices<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string serilogFile,
            KeyValuePair<string,string> dbConnectionStringKey) where TContext : DbContext
        {
            // DB MSSQL 
            if (dbConnectionStringKey.Key.Equals("MSSQL"))
            services.AddDbContext<TContext>(option =>
                option.UseSqlServer(
                    configuration.GetConnectionString(dbConnectionStringKey.Value), sqlserverOption => sqlserverOption.EnableRetryOnFailure()));

            if (dbConnectionStringKey.Key.Equals("MongoDB"))
                services.AddSingleton(new MongoDBService(dbConnectionStringKey.Value));
            //services.AddSingleton<MongoDBService>(dbConnectionStringKey.Value).BuildServiceProvider(); 

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
                .WriteTo.Console(Serilog.Events.LogEventLevel.Error)
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
            // Global Response Handler
            app.UseMiddleware<GlobalResponseHandler>();

            return app;
        }
    }
}
