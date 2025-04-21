using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IOfficeRepository
{
    public Task CreateAsync(Office office);
    public Task UpdateAsync(Guid officeId, Office office);
    public Task<Office> GetByIdAsync(Guid officeId);
    public Task DeleteAsync(Office office);
    public Task SoftDeleteAsync(Office office);
}
