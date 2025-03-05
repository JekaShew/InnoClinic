using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public SpecializationRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddSpecializationAsync(Specialization specialization)
    {
        await _profilesDBContext.Connection.InsertAsync<Specialization>(specialization);
    }

    public async Task DeleteSpecializationByIdAsync(Guid specializationId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Specialization>(new Specialization { Id = specializationId });
    }

    public async Task<ICollection<Specialization>> GetAllSpecializationsAsync()
    {
        var specializations = await _profilesDBContext.Connection.GetAllAsync<Specialization>();
        
        return specializations.ToList();
    }

    public async Task<Specialization> GetSpecializationByIdAsync(Guid specializationId)
    {
        var specialization = await _profilesDBContext.Connection.GetAsync<Specialization>(specializationId);
        
        return specialization;
    }

    public async Task UpdateSpecializationAsync(Specialization updatedSpecialization)
    {
        await _profilesDBContext.Connection.UpdateAsync<Specialization>(updatedSpecialization);
    }
}
