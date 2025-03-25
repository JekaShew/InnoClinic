using InnoClinic.CommonLibrary.Exceptions;
using Microsoft.OpenApi.Models;
using OfficesAPI.Persistance.Extensions;
using OfficesAPI.Services.Extensions;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogMethod(builder.Configuration, builder.Configuration["OfficesSerolog:FileName"]);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
    })
    .AddApplicationPart(typeof(OfficesAPI.Presentation.Controllers.OfficesController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Offices API",
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
        .GetAssembly(typeof(OfficesAPI.Presentation.Controllers.OfficesController));
    var swaggerPath = Path.GetDirectoryName(swaggerAssambly.Location);
    var xmlFile = $"{swaggerAssambly.GetName().Name}.xml";
    var xmlPath = Path.Combine(swaggerPath, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCommonServices(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistanceServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

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