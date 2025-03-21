using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using ProfilesAPI.Persistance.Migrations;
using ProfilesAPI.Persistance.Repositories;
using System.Reflection;

namespace ProfilesAPI.Persistance.Extensions;

public static class PersistanceExtensionMethods
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ProfilesDBContext>();
        services.AddScoped<Database>();

        // Registration of Repositories
        services.AddScoped<IRepositoryManager, DapperRepositoryManager>();

        services.AddFluentMigratorMethod(configuration);

        return services;
    }

    private static IServiceCollection AddFluentMigratorMethod(this IServiceCollection services, IConfiguration configuration)
    {
        // Fluent Migrator
        // ProfilesDBDockerFromLocal
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(configuration.GetConnectionString("ProfilesDBDockerFromLocal"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
            );

        return services;
    }

    public static IApplicationBuilder ApplyFluentMigrationsMethodAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        var database = scope.ServiceProvider.GetRequiredService<Database>();

        var context = migrator.RunnerContext;
        if(context is null)
        {
            database.CreateDatabase("ProfilesDB");
        }

        if (migrator.HasMigrationsToApplyUp())
        {
            migrator.MigrateUp();
        }

        return app;
    }
}
