using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface ISpecializationService
{
    public Task AddSpecializationAsync(SpecializationForCreateDTO specialization);
    public Task UpdateSpecializationAsync(Guid specializationId, SpecializationForUpdateDTO updatedSpecialization);
    public Task DeleteSpecializationByIdAsync(Guid specializationId);
    public Task<SpecializationInfoDTO> GetSpecializationByIdAsync(Guid specializationId);
    public Task<ICollection<SpecializationTableInfoDTO>> GetAllSpecializationsAsync();
}
