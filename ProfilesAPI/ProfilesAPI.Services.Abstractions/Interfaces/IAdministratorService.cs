using ProfilesAPI.Shared.DTOs.AdministratorDTOs;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IAdministratorService
{
    public Task AddAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO);
    public Task UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO);
    public Task DeleteAdministratorByIdAsync(Guid administratorId);
    public Task<AdministratorInfoDTO> GetAdministratorByIdAsync(Guid administratorId);
    public Task<ICollection<AdministratorTableInfoDTO>> GetAllAdministratorsAsync();
}
