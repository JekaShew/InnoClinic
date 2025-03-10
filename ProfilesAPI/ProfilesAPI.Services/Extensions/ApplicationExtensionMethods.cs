using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Services.Services;
using System.Reflection;

namespace ProfilesAPI.Services.Extensions
{
    public static class ApplicationExtensionMethods
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<BlobContainerTitles>()
            .Bind(configuration.GetSection(BlobContainerTitles.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

            // Registration of Services
            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IReceptionistService, ReceptionistService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IWorkStatusService, WorkStatusService>();

            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped(_ => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));

            services.AddFluentValidationMethod();
            services.AddAutoMapperMethod();

            return services;
        }

        private static IServiceCollection AddFluentValidationMethod(this IServiceCollection services)
        {
            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddAutoMapperMethod(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
