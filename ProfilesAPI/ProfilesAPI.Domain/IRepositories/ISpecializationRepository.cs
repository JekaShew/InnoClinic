using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface ISpecializationRepository
{
    public Task CreateAsync(Specialization specialization);
    public Task UpdateAsync(Guid specializationId, Specialization updatedSpecialization);
    public Task DeleteByIdAsync(Guid specializationId);
    public Task<Specialization> GetByIdAsync(Guid specializationId);
    public Task<ICollection<Specialization>> GetAllAsync();
}
