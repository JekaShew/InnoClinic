using ProfilesAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace ProfilesAPI.Domain.IRepositories;

public interface IDoctorRepository
{
    public Task AddDoctorAsync(Doctor doctor);
    public Task UpdateDoctorAsync(Doctor updatedDoctor);
    public Task DeleteDoctorByIdAsync(Guid doctorId);
    public Task<Doctor> GetDoctorByIdAsync(Guid doctorId);
    public Task<ICollection<Doctor>> GetAllDoctorsAsync();
    public Task<ICollection<Doctor>> GetFilteredDoctors(ICollection<Guid> specializtions, ICollection<string> offices, string QueryString);
    public Task<ICollection<Doctor>> GetDoctorsByExpression(Expression<Func<Doctor, bool>> expression);
}
