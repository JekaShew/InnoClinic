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
            services.AddHttpContextAccessor();
            //services.AddScoped<IAuthorizationService, AuthorizationServices>();
            services.AddScoped<IUserService, UserService>();
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
