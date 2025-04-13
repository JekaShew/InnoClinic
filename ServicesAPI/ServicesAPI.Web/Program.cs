using ServicesAPI.Web.WebExtensions;
using InnoClinic.CommonLibrary.Exceptions;
using ServicesAPI.Application.Extensions;
using ServicesAPI.Persistance.Extensions;
using Serilog;
using System.Text.Json.Serialization;
using ServicesAPI.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogMethod(builder.Configuration, builder.Configuration["ServicesSerilog:FileName"]);
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    
})
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddApplicationPart(typeof(ServicesAPI.Presentation.Controllers.ServicesController).Assembly);


builder.Services.AddSwaggerMethod();

builder.Services.AddCommonServices(builder.Configuration);

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);

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

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.Run();
