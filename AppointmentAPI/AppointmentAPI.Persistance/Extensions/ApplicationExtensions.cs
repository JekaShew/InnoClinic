using AppointmentAPI.Persistance.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentAPI.Persistance.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppointmentsDBContext appointmentsDBContext=
                          scope.ServiceProvider.GetRequiredService<AppointmentsDBContext>();
        if (!appointmentsDBContext.Database.CanConnect())
        {
            appointmentsDBContext.Database.Migrate();
        }

        return app;
    }
}
