using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Domain.IRepositories;

public interface IReceptionistRepository
{
    public Task CreateAsync(Receptionist receptionist);
    public Task UpdateAsync(Guid receptionistId, Receptionist updatedReceptionist);
    public Task DeleteByIdAsync(Guid receptionistId);
    public Task<Receptionist> GetByIdAsync(Guid receptionistId);
    public Task<ICollection<Receptionist>> GetAllAsync(ReceptionistParameters? receptionistParameters);
    public Task<bool> IsProfileExists(Guid userId);
}
