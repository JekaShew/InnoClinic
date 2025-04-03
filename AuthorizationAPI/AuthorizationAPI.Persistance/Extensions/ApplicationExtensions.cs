using AuthorizationAPI.Persistance.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationAPI.Persistance.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();


        using AuthDBContext authDBContext =
                          scope.ServiceProvider.GetRequiredService<AuthDBContext>();
        if (!authDBContext.Database.CanConnect())
        {
            authDBContext.Database.Migrate();
        }

        return app;
    }
}