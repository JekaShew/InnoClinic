using InnoClinic.CommonLibrary.Response;
using OfficesAPI.Shared.DTOs;

namespace OfficesAPI.Services.Abstractions.Interfaces
{
    public interface IOfficeService
    {
        public Task<ResponseMessage<IEnumerable<OfficeTableInfoDTO>>> GetAllOfficesAsync();
        public Task<ResponseMessage<OfficeInfoDTO>> GetOfficeByIdAsync(string officeId);
        public Task<ResponseMessage> CreateOfficeAsync(OfficeForCreateDTO officeForCreateDTO);
        public Task<ResponseMessage> UpdateOfficeAsync(string officeId, OfficeForUpdateDTO officeForUpdateDTO);
        public Task<ResponseMessage> DeleteOfficeByIdAsync(string officeId);
        public Task<ResponseMessage> ChangeStatusOfOfficeByIdAsync(string officeId);
    }
}
