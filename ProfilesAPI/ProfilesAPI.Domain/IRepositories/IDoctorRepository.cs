using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using System.Linq.Expressions;

namespace ProfilesAPI.Domain.IRepositories;

public interface IDoctorRepository
{
    public Task CreateAsync(Doctor doctor);
    public Task UpdateAsync(Guid doctorId, Doctor updatedDoctor);
    public Task DeleteByIdAsync(Guid doctorId);
    public Task<Doctor> GetByIdAsync(Guid doctorId);
    public Task<ICollection<Doctor>> GetAllAsync(DoctorParameters? doctorParameters);
    public Task<bool> IsProfileExists(Guid userId);   
    public Task DeleteSelectedSpecializationsByDoctorIdAsync(Guid doctorId);
    public Task AddSelectedSpecializationAsync(Guid doctorId, ICollection<DoctorSpecialization> doctorSpecializationsToAdd);
}
