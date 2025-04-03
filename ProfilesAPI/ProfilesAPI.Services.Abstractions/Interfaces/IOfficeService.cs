using CommonLibrary.RabbitMQEvents;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IOfficeService
{
    public Task<ResponseMessage> RequestCheckOfficeConsistancyAsync();
}
