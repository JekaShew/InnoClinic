using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public ReceptionistRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddReceptionistAsync(Receptionist receptionist)
    {
        await _profilesDBContext.Connection.InsertAsync<Receptionist>(receptionist);
    }

    public async Task DeleteReceptionistByIdAsync(Guid receptionistId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Receptionist>(new Receptionist { UserId = receptionistId });
    }

    public async Task<ICollection<Receptionist>> GetAllReceptionistsAsync()
    {
        var receptionists = await _profilesDBContext.Connection.GetAllAsync<Receptionist>();

        return receptionists.ToList();
    }

    public async Task<Receptionist> GetReceptionistByIdAsync(Guid receptionistId)
    {
        var receptionist = await _profilesDBContext.Connection.GetAsync<Receptionist>(receptionistId);

        return receptionist;
    }

    public async Task UpdateReceptionistAsync(Receptionist updatedReceptionist)
    {
        await _profilesDBContext.Connection.UpdateAsync<Receptionist>(updatedReceptionist);
    }
}
