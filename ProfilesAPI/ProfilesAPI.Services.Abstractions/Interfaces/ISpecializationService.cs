using CommonLibrary.RabbitMQEvents.OfficeEvents;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using InnoClinic.CommonLibrary.Response;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface ISpecializationService
{
    public Task<ResponseMessage> RequestCheckSpecializationConsistancyAsync();
    public Task CreateSpecializationAsync(SpecializationCreatedEvent specializationCreatedEvent);
    public Task CheckSpecializationConsistancyAsync(SpecializationCheckConsistancyEvent specializationCheckConsistancyEvent);
    public Task UpdateSpecializationAsync(SpecializationUpdatedEvent specializationUpdatedEvent);
    public Task DeleteSpecializationAsync(SpecializationDeletedEvent specializationDeletedEvent);
}
