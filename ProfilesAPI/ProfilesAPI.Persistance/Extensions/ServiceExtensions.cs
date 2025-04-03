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

public static class ServiceExtensions
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
                .WithGlobalConnectionString(configuration.GetConnectionString("ProfilesDBDocker"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
            );

        return services;
    }
}
