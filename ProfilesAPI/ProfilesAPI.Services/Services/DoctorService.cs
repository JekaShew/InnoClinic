using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepositoryManager _repositoryManager;

    public DoctorService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public Task AddDoctorAsync(DoctorForCreateDTO doctorForCreateDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDoctorByIdAsync(Guid doctorId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DoctorTableInfoDTO>> GetAllDoctorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DoctorInfoDTO> GetDoctorByIdAsync(Guid doctorId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDoctorAsync(Guid doctorId, DoctorForUpdateDTO doctorForUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
