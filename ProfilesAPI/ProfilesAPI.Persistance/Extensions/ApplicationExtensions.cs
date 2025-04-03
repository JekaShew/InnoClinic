using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProfilesAPI.Persistance.Migrations;

namespace ProfilesAPI.Persistance.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ApplyFluentMigrationsMethodAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        var database = scope.ServiceProvider.GetRequiredService<Database>();

        var context = migrator.RunnerContext;
        if (context is null)
        {
            database.CreateDatabase("ProfilesDB");
        }

        if (migrator.HasMigrationsToApplyUp())
        {
            migrator.MigrateUp();
        }

        return app;
    }
}
