using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ServicesAPI.Web.WebExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSwaggerMethod(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                         new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
                });
                // Add xml Swagger comments
                var swaggerAssambly = Assembly
                    .GetAssembly(typeof(ServicesAPI.Presentation.Controllers.ServiceController));
                var swaggerPath = Path.GetDirectoryName(swaggerAssambly.Location);
                var xmlFile = $"{swaggerAssambly.GetName().Name}.xml";
                var xmlPath = Path.Combine(swaggerPath, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            return services;
        }
    }
}
