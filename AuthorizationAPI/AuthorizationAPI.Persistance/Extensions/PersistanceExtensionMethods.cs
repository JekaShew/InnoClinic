using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using AuthorizationAPI.Persistance.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationAPI.Persistance.Extensions;

public static class PersistanceExtensionMethods
{

    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddMSSQLDBContextMethod(services, configuration);
        AddPersistanceRepostitoriesMethod(services);
    
        return services;
    }

    private static IServiceCollection AddMSSQLDBContextMethod(this IServiceCollection services, IConfiguration configuration)
    {
        // MSSQL DB
        services.AddDbContext<AuthDBContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AuthDBDocker"),
                sqlserverOption =>
                {
                    // if Migrations in different Assembly
                    sqlserverOption.MigrationsAssembly("AuthorizationAPI.Persistance");
                    sqlserverOption.EnableRetryOnFailure();
                }));

            return services;
    }

    private static IServiceCollection AddPersistanceRepostitoriesMethod(this IServiceCollection services)
    {
        // Registration of Repositories
        services.AddScoped<IRepositoryManager, EFCoreRepositoryManger>();

        return services;
    }

    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        
        using AuthDBContext authDBContext =
                          scope.ServiceProvider.GetRequiredService<AuthDBContext>();
        if(!authDBContext.Database.CanConnect())
        {
            authDBContext.Database.Migrate();
        }    

            return app;
    }
}
