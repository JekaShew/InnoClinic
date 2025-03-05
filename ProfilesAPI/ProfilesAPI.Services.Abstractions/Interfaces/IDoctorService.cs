using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IDoctorService
{
    public Task AddDoctorAsync(DoctorForCreateDTO doctorForCreateDTO);
    public Task UpdateDoctorAsync(Guid doctorId, DoctorForUpdateDTO doctorForUpdateDTO);
    public Task DeleteDoctorByIdAsync(Guid doctorId);
    public Task<DoctorInfoDTO> GetDoctorByIdAsync(Guid doctorId);
    public Task<ICollection<DoctorTableInfoDTO>> GetAllDoctorsAsync();
}
