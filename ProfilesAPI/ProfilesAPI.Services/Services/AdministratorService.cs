using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IRepositoryManager _repositoryManager;
    public AdministratorService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public Task AddAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAdministratorByIdAsync(Guid administratorId)
    {
        throw new NotImplementedException();
    }

    public Task<AdministratorInfoDTO> GetAdministratorByIdAsync(Guid administratorId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<AdministratorTableInfoDTO>> GetAllAdministratorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
