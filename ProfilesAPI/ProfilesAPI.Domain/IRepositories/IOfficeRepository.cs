using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IOfficeRepository
{
    public Task AddSpecializationAsync(Specialization specialization);
}
