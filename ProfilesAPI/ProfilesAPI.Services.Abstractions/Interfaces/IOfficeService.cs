using CommonLibrary.RabbitMQEvents.OfficeEvents;
using InnoClinic.CommonLibrary.Response;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IOfficeService
{
    public Task<ResponseMessage> RequestCheckOfficeConsistancyAsync();
    public Task CreateOfficeAsync(OfficeCreatedEvent officeCreatedEvent);
    public Task CheckOfficeConsistancyAsync(OfficeCheckConsistancyEvent officeCheckConsistancyEvent);
    public Task UpdateOfficeAsync(OfficeUpdatedEvent officeUpdatedEvent);
    public Task DeleteOfficeAsync(OfficeDeletedEvent officeDeletedEvent);
}
