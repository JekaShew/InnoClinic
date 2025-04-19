using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentAPI.Persistance.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddMSSQLDBContextMethod(services, configuration);
        AddPersistanceRepostitoriesMethod(services);

        return services;
    }

    private static IServiceCollection AddMSSQLDBContextMethod(this IServiceCollection services, IConfiguration configuration)
    {
        // MSSQL DB
        services.AddDbContext<AppointmentsDBContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AppointmentsDBDocker"),
                sqlserverOption =>
                {
                    // if Migrations in different Assembly
                    sqlserverOption.MigrationsAssembly("AppointmentAPI.Persistance");
                    sqlserverOption.EnableRetryOnFailure();
                }));

        return services;
    }

    private static IServiceCollection AddPersistanceRepostitoriesMethod(this IServiceCollection services)
    {
        // Registration of Repositories
        services.AddScoped<IRepositoryManager, EFCoreRepositoryManger>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(EFGenericRepository<>));

        return services;
    }
}
