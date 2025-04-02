using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Shared.DTOs.OfficeDTOs;

namespace OfficesAPI.Services.Abstractions.Interfaces;

public interface IOfficeService
{
    public Task<ResponseMessage<IEnumerable<OfficeTableInfoDTO>>> GetAllOfficesAsync();
    public Task<ResponseMessage<OfficeInfoDTO>> GetOfficeByIdAsync(string officeId);
    public Task<ResponseMessage<OfficeInfoDTO>> CreateOfficeAsync(OfficeForCreateDTO officeForCreateDTO, ICollection<IFormFile> files);
    public Task<ResponseMessage<OfficeInfoDTO>> UpdateOfficeInfoAsync(string officeId, OfficeForUpdateDTO officeForUpdateDTO);
    public Task<ResponseMessage> DeleteOfficeByIdAsync(string officeId);
    public Task<ResponseMessage> ChangeStatusOfOfficeByIdAsync(string officeId);
}
