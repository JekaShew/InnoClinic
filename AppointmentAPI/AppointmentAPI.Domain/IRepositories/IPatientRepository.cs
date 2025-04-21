using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Domain.IRepositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    public Task SoftDeleteAsync(Patient patient);
}
