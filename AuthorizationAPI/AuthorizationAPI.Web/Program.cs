using AuthorizationAPI.Persistance.Extensions;
using AuthorizationAPI.Services.Extensions;
using InnoClinic.CommonLibrary.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

CommonServicesExtensions.AddCommonServices(builder.Services, builder.Configuration, builder.Configuration["AuthSerolog:FileName"]!);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistanceServices(builder.Configuration);

var app = builder.Build();

CommonServicesExtensions.UseCommonPolicies(app);

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
