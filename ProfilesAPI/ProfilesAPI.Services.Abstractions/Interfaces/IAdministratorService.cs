using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IAdministratorService
{
    public Task<ResponseMessage> AddAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO);
    public Task<ResponseMessage> UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO);
    public Task<ResponseMessage> DeleteAdministratorByIdAsync(Guid administratorId);
    public Task<ResponseMessage<AdministratorInfoDTO>> GetAdministratorByIdAsync(Guid administratorId);
    public Task<ResponseMessage<ICollection<AdministratorTableInfoDTO>>> GetAllAdministratorsAsync();
}
