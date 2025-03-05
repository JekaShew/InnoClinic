using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Services;

public class SpecializationService : ISpecializationService
{
    private readonly IRepositoryManager _repositoryManager;

    public SpecializationService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public Task AddSpecializationAsync(SpecializationForCreateDTO specialization)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSpecializationByIdAsync(Guid specializationId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<SpecializationTableInfoDTO>> GetAllSpecializationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<SpecializationInfoDTO> GetSpecializationByIdAsync(Guid specializationId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSpecializationAsync(Guid specializationId, SpecializationForUpdateDTO updatedSpecialization)
    {
        throw new NotImplementedException();
    }
}
