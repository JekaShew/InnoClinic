using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AuthorizationAPI.Services.Extensions
{
    public static class ApplicationServicesExtesionMethods
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationSettings>(configuration.GetSection("Authentication"));

            //services.AddScoped<IUserStatusService, UserStatusService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddFluentValidationMethod(configuration);

            return services;
        }

        private static IServiceCollection AddFluentValidationMethod(this IServiceCollection services, IConfiguration configuration)
        {
            // FluentValidation
            //services.AddValidatorsFromAssembly(typeof(AddUserStatusCommandValidator).Assembly);

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
