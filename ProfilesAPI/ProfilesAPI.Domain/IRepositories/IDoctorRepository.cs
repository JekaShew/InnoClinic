using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using System.Linq.Expressions;

namespace ProfilesAPI.Domain.IRepositories;

public interface IDoctorRepository
{
    public Task AddDoctorAsync(Doctor doctor);
    public Task UpdateDoctorAsync(Guid doctorId, Doctor updatedDoctor);
    public Task DeleteDoctorByIdAsync(Guid doctorId);
    public Task<Doctor> GetDoctorByIdAsync(Guid doctorId);
    public Task<ICollection<Doctor>> GetAllDoctorsAsync(DoctorParameters? doctorParameters);
    public Task<bool> IsProfileExists(Guid userId);
    //public Task<ICollection<Doctor>> GetFilteredDoctors(ICollection<Guid> specializtions, ICollection<string> offices, string QueryString);
    //public Task<ICollection<Doctor>> GetFilteredDoctors();
    
    public Task<ICollection<Doctor>> GetDoctorsByExpression(Expression<Func<Doctor, bool>> expression);
    public Task DeleteSelectedDoctorSpecializationsByDoctorIdAsync(Guid doctorId);
    public Task AddSelectedDoctorSpecializationAsync(Guid doctorId, ICollection<DoctorSpecialization> doctorSpecializationsToAdd);
}
