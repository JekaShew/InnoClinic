using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Domain.IRepositories;

public interface IReceptionistRepository
{
    public Task AddReceptionistAsync(Receptionist receptionist);
    public Task UpdateReceptionistAsync(Guid receptionistId, Receptionist updatedReceptionist);
    public Task DeleteReceptionistByIdAsync(Guid receptionistId);
    public Task<Receptionist> GetReceptionistByIdAsync(Guid receptionistId);
    public Task<ICollection<Receptionist>> GetAllReceptionistsAsync(ReceptionistParameters? receptionistParameters);
    public Task<bool> IsProfileExists(Guid userId);
}
