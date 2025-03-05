using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface ISpecializationRepository
{
    public Task AddSpecializationAsync(Specialization specialization);
    public Task UpdateSpecializationAsync(Specialization updatedSpecialization);
    public Task DeleteSpecializationByIdAsync(Guid specializationId);
    public Task<Specialization> GetSpecializationByIdAsync(Guid specializationId);
    public Task<ICollection<Specialization>> GetAllSpecializationsAsync();
}
