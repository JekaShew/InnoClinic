using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Services;

public class PatientService : IPatientService
{
    private readonly IRepositoryManager _repositoryManager;

    public PatientService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public Task AddPatientAsync(PatientForCreateDTO patientForCreateDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeletePatientByIdAsync(Guid patientId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<PatientTableInfoDTO>> GetAllPatientsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PatientInfoDTO> GetPatientByIdAsync(Guid patientId)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePatientAsync(Guid patientId, PatientForUpdateDTO patientForUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
