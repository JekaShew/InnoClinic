using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IAdministratorService
{
    public Task<ResponseMessage> AddAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO, IFormFile file);
    public Task<ResponseMessage> UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO, IFormFile? file);
    public Task<ResponseMessage> DeleteAdministratorByIdAsync(Guid administratorId);
    public Task<ResponseMessage<AdministratorInfoDTO>> GetAdministratorByIdAsync(Guid administratorId);
    public Task<ResponseMessage<ICollection<AdministratorTableInfoDTO>>> GetAllAdministratorsAsync();
}
