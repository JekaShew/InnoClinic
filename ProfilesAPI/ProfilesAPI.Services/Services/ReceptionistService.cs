using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IRepositoryManager _repositoryManager;

    public ReceptionistService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public Task AddReceptionistAsync(ReceptionistForCreateDTO receptionistForCreateDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteReceptionistByIdAsync(Guid receptionistId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ReceptionistTableInfoDTO>> GetAllReceptionistsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ReceptionistInfoDTO> GetReceptionistByIdAsync(Guid receptionistId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateReceptionistAsync(Guid receptionistId, ReceptionistForUpdateDTO receptionistForUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
