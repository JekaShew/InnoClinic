using AppointmentAPI.Persistance.Extensions;
using AppointmentAPI.Presentation.Extensions;
using AppointmentAPI.Web.Extensions;
using InnoClinic.CommonLibrary.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogMethod(builder.Configuration, builder.Configuration["AppointmentsSerilog:FileName"]);
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
})
    .AddApplicationPart(typeof(AppointmentAPI.Presentation.Controllers.AppointmentsController).Assembly);

builder.Services.AddSwaggerMethod();
builder.Services.AddCorsPolicies();

builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCommonPolicies();

app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment API");
});

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();