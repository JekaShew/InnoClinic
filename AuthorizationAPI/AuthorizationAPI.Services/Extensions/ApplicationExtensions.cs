using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace AuthorizationAPI.Services.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder StartBackgroundTasks(this IApplicationBuilder app, IConfiguration configuration)
    {
        // Schedule Jobs
        var crons = configuration.GetSection("HangFire:Crons");
        IRecurringJobManager recurringJobManager = new RecurringJobManager();
        recurringJobManager.AddOrUpdate<BackgroundTasks>(
                "CleaningExpiredRefresTokens",
                x => x.CleanExpiredRefreshTokensAsync(),
                crons["Every15Minutes"]);

        return app;
    }
}
