using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Domain.IRepositories;

public interface IAdministratorRepository
{
    public Task CreateAsync(Administrator administrator);
    public Task UpdateAsync(Guid administratorId, Administrator updatedAdministrator);
    public Task DeleteByIdAsync(Guid administratorId);
    public Task<Administrator> GetByIdAsync(Guid administratorId);
    public Task<ICollection<Administrator>> GetAllAsync(AdministratorParameters? administratorParameters);
    public Task<bool> IsProfileExists(Guid userId);
}
