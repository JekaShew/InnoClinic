using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Domain.IRepositories;

public interface IDoctorSpecializationRepository : IGenericRepository<DoctorSpecialization>
{
    public Task SoftDeleteAsync(DoctorSpecialization doctorSpecialization);
}
