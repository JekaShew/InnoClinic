using AuthorizationAPI.Persistance.Extensions;
using InnoClinic.CommonLibrary.Exceptions;
using AuthorizationAPI.Services.Extensions;
using Hangfire;
using Serilog;
using AuthorizationAPI.Web.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogMethod(builder.Configuration, builder.Configuration["AuthSerilog:FileName"]);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
    })
    .AddNewtonsoftJson()
    .AddApplicationPart(typeof(AuthorizationAPI.Presentation.Controllers.UsersController).Assembly);

        
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization API");
});

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();
        
app.ApplyMigrations();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Authorization API Jobs",
    //Authorization = new[]
    //{
    //    new HangfireCustomBasicAuthenticationFilter
    //    {
    //        User = "Administrator",
    //        Pass = "qwedsazxc123"
    //    }
    //}
});
app.StartBackgroundTasks(builder.Configuration);
        
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.Run();
