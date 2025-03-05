using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IAdministratorRepository
{
    public Task AddAdministratorAsync(Administrator administrator);
    public Task UpdateAdministratorAsync(Administrator updatedAdministrator);
    public Task DeleteAdministratorByIdAsync(Guid administratorId);
    public Task<Administrator> GetAdministratorByIdAsync(Guid administratorId);
    public Task<ICollection<Administrator>> GetAllAdministratorsAsync();
}
