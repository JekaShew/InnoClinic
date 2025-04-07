using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IOfficeRepository
{
    public Task CreateAsync(Office office);
    public Task UpdateAsync(string officeId, Office office);
    public Task<Office> GetByIdAsync(string officeId);
    public Task DeleteAsync(Office office);
}
