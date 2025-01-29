using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.Validators.UserStatusValidators;
using AuthorizationAPI.Domain.Data;
using FluentValidation;
using InnoShop.CommonLibrary.DependencyInjection;
using InnoShop.CommonLibrary.Middleware;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // last variable is for DB Connection String Key that in configuration
            CommonServiceContainer.AddCommonServices<AuthDBContext>(services, configuration, configuration["AuthSerolog:FileName"]!, "AuthDB");

            //MediatR
            services.AddMediatR(cfg => cfg
                        .RegisterServicesFromAssembly(typeof(TakeRefreshTokenDTOByRTokenIdQuery).Assembly));

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(AddUserStatusCommandValidator).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            CommonServiceContainer.UseCommonPolicies(app);

            return app;
        }
    }
}
