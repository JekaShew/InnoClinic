using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using AuthorizationAPI.Persistance.Repositories;
using InnoClinic.CommonLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationAPI.Persistance.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // last variable is for DB Connection String Key that in configuration
            CommonServiceContainer.AddCommonServices<AuthDBContext>(services, configuration, configuration["AuthSerolog:FileName"]!, new KeyValuePair<string, string>("MSSQL", "AuthDB"));

            //Registration of Repositories
            services.AddScoped<IUserStatusRepository, UserStatusRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            ////MediatR
            //services.AddMediatR(cfg => cfg
            //            .RegisterServicesFromAssembly(typeof(TakeUserStatusDTOByIdQueryHandler).Assembly));

            //// FluentValidation
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
