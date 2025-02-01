using InnoClinic.CommonLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Repositories;

namespace OfficesAPI.Persistance.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {


            // last variable is for DB Connection String Key that in configuration
            // ??????? think about it zaglushka s DbContext govno ????????
            CommonServiceContainer.AddCommonServices<DbContext>(
                    services,
                    configuration,
                    configuration["OfficesSerolog:FileName"]!,
                    new KeyValuePair<string, string> ( "MongoDB", "OfficesDB"));

            //Registration of Repositories
            services.AddScoped<IOffice, OfficeRepository>();
            services.AddScoped<IPhoto, PhotoRepository>();

            // FluentValidation
            //services.AddValidatorsFromAssembly(typeof(AddUserStatusCommandValidator).Assembly);

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            CommonServiceContainer.UseCommonPolicies(app);

            return app;
        }
    }
}
