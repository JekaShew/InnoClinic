using CommonLibrary.RabbitMQEvents.OfficeEvents;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Shared.DTOs.OfficeDTOs;

namespace OfficesAPI.Services.Abstractions.Interfaces;

public interface IOfficeService
{
    public Task<ResponseMessage<IEnumerable<OfficeTableInfoDTO>>> GetAllOfficesAsync();
    public Task<IEnumerable<OfficeCheckConsistancyEvent>> GetAllOfficeCheckConsistancyEventsAsync();
    public Task<ResponseMessage<OfficeInfoDTO>> GetOfficeByIdAsync(Guid officeId);
    public Task<ResponseMessage<OfficeInfoDTO>> CreateOfficeAsync(OfficeForCreateDTO officeForCreateDTO, ICollection<IFormFile> files);
    public Task<ResponseMessage<OfficeInfoDTO>> UpdateOfficeInfoAsync(Guid officeId, OfficeForUpdateDTO officeForUpdateDTO);
    public Task<ResponseMessage> DeleteOfficeByIdAsync(Guid officeId);
    public Task<ResponseMessage> ChangeStatusOfOfficeByIdAsync(Guid officeId);
}
