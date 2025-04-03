using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Data;
using OfficesAPI.Persistance.Repositories;

namespace OfficesAPI.Persistance.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ConnectionStringsSettings>()
            .Bind(configuration.GetSection(ConnectionStringsSettings.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        //Registration of Repositories
        services.AddScoped<IOfficesContext, OfficesContext>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }
}
