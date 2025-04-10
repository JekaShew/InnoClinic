using InnoClinic.CommonLibrary.Exceptions;
using OfficesAPI.Persistance.Extensions;
using OfficesAPI.Services.Extensions;
using OfficesAPI.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogMethod(builder.Configuration, builder.Configuration["OfficesSerilog:FileName"]);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
    })
    .AddApplicationPart(typeof(OfficesAPI.Presentation.Controllers.OfficesController).Assembly);

builder.Services.AddSwaggerMethod();
builder.Services.AddCorsPolicies();

builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistanceServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCommonPolicies();

app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Offices API");
});

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();