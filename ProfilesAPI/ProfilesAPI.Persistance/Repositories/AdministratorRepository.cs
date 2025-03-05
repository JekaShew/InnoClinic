using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly ProfilesDBContext _profilesDBContext;
    public AdministratorRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddAdministratorAsync(Administrator administrator)
    {
        await _profilesDBContext.Connection.InsertAsync<Administrator>(administrator);
    }

    public async Task DeleteAdministratorByIdAsync(Guid administratorId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Administrator>(new Administrator { UserId = administratorId });
    }

    public async Task<Administrator> GetAdministratorByIdAsync(Guid administratorId)
    {
        var administrator = await _profilesDBContext.Connection.GetAsync<Administrator>(administratorId);

        return administrator;
    }

    public async Task<ICollection<Administrator>> GetAllAdministratorsAsync()
    {
        var administrators = await _profilesDBContext.Connection.GetAllAsync<Administrator>();

        return administrators.ToList();
    }

    public async Task UpdateAdministratorAsync(Administrator updatedAdministrator)
    {
        await _profilesDBContext.Connection.UpdateAsync<Administrator>(updatedAdministrator);
    }
}
