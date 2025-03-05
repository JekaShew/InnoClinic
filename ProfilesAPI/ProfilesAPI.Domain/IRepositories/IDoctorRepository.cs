using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IDoctorRepository
{
    public Task AddDoctorAsync(Doctor doctor);
    public Task UpdateDoctorAsync(Doctor updatedDoctor);
    public Task DeleteDoctorByIdAsync(Guid doctorId);
    public Task<Doctor> GetDoctorByIdAsync(Guid doctorId);
    public Task<ICollection<Doctor>> GetAllDoctorsAsync();
}
