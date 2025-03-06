using Microsoft.OpenApi.Models;
using System.Reflection;
using ProfilesAPI.Persistance.Extensions;
using InnoClinic.CommonLibrary.Exceptions;
using ProfilesAPI.Services.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        })
            .AddApplicationPart(typeof(ProfilesAPI.Presentation.Controllers.SpecializationController).Assembly);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Profiles API",
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

            var swaggerAssambly = Assembly
                .GetAssembly(typeof(ProfilesAPI.Presentation.Controllers.SpecializationController));
            var swaggerPath = Path.GetDirectoryName(swaggerAssambly.Location);
            var xmlFile = $"{swaggerAssambly.GetName().Name}.xml";
            var xmlPath = Path.Combine(swaggerPath, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddCommonServices(builder.Configuration, builder.Configuration["ProfilesSerilog:FileName"]);
        
        builder.Services.AddPersistanceServices(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        });

        var app = builder.Build();

        app.UseCommonPolicies();
        
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles API");
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.ApplyFluentMigrationsMethodAsync();    

        app.MapControllers();
        app.Run();
    }
}