using Azure.Storage.Blobs;
using CommonLibrary.RabbitMQEvents;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Services.Services;
using ProfilesAPI.Services.Services.OfficeConsumers;
using ProfilesAPI.Services.Services.SpecializationConsumers;
using System.Reflection;

namespace ProfilesAPI.Services.Extensions
{
    public static class ServiceExtensions
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
            services.AddScoped<IOfficeService, OfficeService>();

            services.AddAzureBlobStorageMethod(configuration);
            services.AddFluentValidationMethod();
            services.AddAutoMapperMethod();
            services.AddRabbitMQMethod(configuration);

            return services;
        }

        private static IServiceCollection AddRabbitMQMethod(this IServiceCollection services, IConfiguration configuration)
        {
            // RabbitMQ
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                    
                busConfigurator.AddConsumer<OfficeCreatedConsumer>();
                busConfigurator.AddConsumer<OfficeDeletedConsumer>();
                busConfigurator.AddConsumer<OfficeUpdatedConsumer>();
                busConfigurator.AddConsumer<OfficeCheckConsistancyConsumer>();

                busConfigurator.AddConsumer<SpecializationCreatedConsumer>();
                busConfigurator.AddConsumer<SpecializationDeletedConsumer>();
                busConfigurator.AddConsumer<SpecializationUpdatedConsumer>();
                busConfigurator.AddConsumer<SpecializationCheckConsistancyConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration["MessageBroker:HostDocker"], 5672, "/", hostConfigurator =>
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
            services.AddScoped(_ => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));

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
