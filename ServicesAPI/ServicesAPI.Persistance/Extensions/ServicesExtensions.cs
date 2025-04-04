using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Persistance.Repositories;

namespace ServicesAPI.Persistance.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddPostgreSQLDBContextMethod(services, configuration);
        AddPersistanceRepostitoriesMethod(services);

        return services;
    }

    private static IServiceCollection AddPostgreSQLDBContextMethod(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL DB
        services.AddDbContext<ServicesDBContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("ServicesDBDocker"),
                postgresqlserverOption =>
                {
                    // if Migrations in different Assembly
                    postgresqlserverOption.MigrationsAssembly("ServicesAPI.Persistance");
                    postgresqlserverOption.EnableRetryOnFailure();
                }));

        return services;
    }

    private static IServiceCollection AddPersistanceRepostitoriesMethod(this IServiceCollection services)
    {
        // Registration of Repositories
        services.AddScoped<IRepositoryManager, EFCoreRepositoryManager>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
