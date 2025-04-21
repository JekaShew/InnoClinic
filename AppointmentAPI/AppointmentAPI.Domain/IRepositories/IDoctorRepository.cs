using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Domain.IRepositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    public Task SoftDeleteAsync(Doctor doctor);
}
