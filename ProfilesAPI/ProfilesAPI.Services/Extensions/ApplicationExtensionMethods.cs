using Azure.Storage.Blobs;
using CommonLibrary.RabbitMQEvents;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Services.Services;
using ProfilesAPI.Services.Services.OfficesConsumers;
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

            services.AddAzureBlobStorageMethod(configuration);
            services.AddFluentValidationMethod();
            services.AddAutoMapperMethod();
            services.AddRabbitMQMethod(configuration);

            return services;
        }

        private static IServiceCollection AddRabbitMQMethod(this IServiceCollection services, IConfiguration configuration)
        {
            // FluentValidation
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                    
                busConfigurator.AddConsumer<OfficeCreatedConsumer>();
                busConfigurator.AddConsumer<OfficeUpdatedConsumer>();
                busConfigurator.AddConsumer<OfficeCheckConsistancyConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]), hostConfigurator =>
                    {
                        hostConfigurator.Username(configuration["MessageBroker:Username"]);
                        hostConfigurator.Password(configuration["MessageBroker:Password"]);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        private static IServiceCollection AddFluentValidationMethod(this IServiceCollection services)
        {
            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddAzureBlobStorageMethod(this IServiceCollection services, IConfiguration configuration)
        {
            // Azure Blob Storage
            // AzureBlobStorageFromLocal
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped(_ => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorageFromLocal")));

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
