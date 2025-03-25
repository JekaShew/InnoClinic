using Microsoft.OpenApi.Models;
using System.Reflection;
using ProfilesAPI.Persistance.Extensions;
using InnoClinic.CommonLibrary.Exceptions;
using ProfilesAPI.Services.Extensions;
using ProfilesAPI.Web.Extensions;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.AddSerilogMethod(builder.Configuration, builder.Configuration["ProfilesSerilog:FileName"]);

        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
        })
            .AddApplicationPart(typeof(ProfilesAPI.Presentation.Controllers.SpecializationsController).Assembly);

        builder.Services.AddSwaggerMethod();

        builder.Services.AddCommonServices(builder.Configuration);
        
        builder.Services.AddPersistanceServices(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCorsPolicies();

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

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseAuthorization();

        app.ApplyFluentMigrationsMethodAsync();    

        app.MapControllers();
        app.Run();
    }
}