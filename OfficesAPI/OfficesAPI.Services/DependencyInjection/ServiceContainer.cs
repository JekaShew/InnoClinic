using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Services.Services;

namespace OfficesAPI.Services.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IOfficeServices, OfficeServices>();
            services.AddScoped<IPhotoServices, PhotoServices>();

            return services;
        }

    }
}
