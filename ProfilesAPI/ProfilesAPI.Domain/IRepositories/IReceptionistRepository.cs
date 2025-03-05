using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IReceptionistRepository
{
    public Task AddReceptionistAsync(Receptionist receptionist);
    public Task UpdateReceptionistAsync(Receptionist updatedReceptionist);
    public Task DeleteReceptionistByIdAsync(Guid receptionistId);
    public Task<Receptionist> GetReceptionistByIdAsync(Guid receptionistId);
    public Task<ICollection<Receptionist>> GetAllReceptionistsAsync();
}
