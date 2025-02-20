using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Repositories;

namespace OfficesAPI.Persistance.Extensions;

public static class PersistanceExtensionMethods
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {

        //Registration of Repositories
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();

        return services;
    }
}
