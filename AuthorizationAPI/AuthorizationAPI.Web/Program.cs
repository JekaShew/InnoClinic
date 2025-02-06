using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Repositories;
using AuthorizationAPI.Persistance.Extensions;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AuthorizationAPI.Presentation.Controllers.UserStatusesController).Assembly);
builder.Services.AddMSSQLDBContextMethod(builder.Configuration);
builder.Services.AddScoped<IRepositoryManager, RepositoryManger>();
builder.Services.AddScoped<IUserStatusService, UserStatusService>(); // Замените на реальную реализацию

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();

//using AuthorizationAPI.Persistance.Extensions;
//using AuthorizationAPI.Services.Extensions;
//using AuthorizationAPI.Presentation;
//using InnoClinic.CommonLibrary.Exceptions;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Mvc.ApplicationParts;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers()
//    .ConfigureApplicationPartManager(manager =>
//    {
//        var assembly = typeof(AuthorizationAPI.Presentation.Controllers.UserStatusesController).Assembly;
//        var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
//        foreach (var part in partFactory.GetApplicationParts(assembly))
//        {
//            manager.ApplicationParts.Add(part);
//        }
//    });
////.AddApplicationPart(typeof(AuthorizationAPI.Presentation.Controllers.UserStatusesController).Assembly);

////builder.Services.AddPresentationControllers();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "Authorization API",
//        Version = "v1"
//    });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "place to add JWT with Bearer",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                },
//                Name = "Bearer",
//            },
//            new List<string>()
//        }
//    });
//});

//CommonServicesExtensions.AddCommonServices(builder.Services, builder.Configuration, builder.Configuration["AuthSerolog:FileName"]);

//builder.Services.AddPersistanceServices(builder.Configuration);
//builder.Services.AddApplicationServices(builder.Configuration);

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", builder =>
//    builder.AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader());
//});



//var app = builder.Build();

//CommonServicesExtensions.UseCommonPolicies(app);

//app.UseStaticFiles();

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization API");
//});

//app.UseHttpsRedirection();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();
