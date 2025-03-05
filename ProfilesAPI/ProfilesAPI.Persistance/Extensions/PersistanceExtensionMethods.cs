using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Repositories;

namespace ProfilesAPI.Persistance.Extensions;

public static class PersistanceExtensionMethods
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Registration of Repositories
        services.AddScoped<IRepositoryManager, DapperRepositoryManager>();

        return services;
    }


   

}
