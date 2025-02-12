using AuthorizationAPI.Persistance.Extensions;
using InnoClinic.CommonLibrary.Exceptions;
using AuthorizationAPI.Services.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    })
    .AddNewtonsoftJson()
    .AddApplicationPart(typeof(AuthorizationAPI.Presentation.Controllers.UsersController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Authorization API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "place to add JWT with Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
            },
            new List<string>()
        }
    });

    var swaggerAssambly = Assembly
        .GetAssembly(typeof(AuthorizationAPI.Presentation.Controllers.AuthorizationController));
    var swaggerPath = Path.GetDirectoryName(swaggerAssambly.Location);
    var xmlFile = $"{swaggerAssambly.GetName().Name}.xml";
    var xmlPath = Path.Combine(swaggerPath, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

CommonServicesExtensions.AddCommonServices(builder.Services, builder.Configuration, builder.Configuration["AuthSerolog:FileName"]);

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

var app = builder.Build();

CommonServicesExtensions.UseCommonPolicies(app);

app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization API");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.Run();

