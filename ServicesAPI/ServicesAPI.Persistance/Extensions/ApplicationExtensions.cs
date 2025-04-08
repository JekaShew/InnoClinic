using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ServicesDBContext servicesDBContext =
                          scope.ServiceProvider.GetRequiredService<ServicesDBContext>();
        if (!servicesDBContext.Database.CanConnect())
        {
            servicesDBContext.Database.Migrate();
        }

        return app;
    }
}
