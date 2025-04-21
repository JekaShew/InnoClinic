using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Domain.IRepositories;

public interface ISpecializationRepository : IGenericRepository<Specialization>
{
    public Task SoftDeleteAsync(Specialization specialization);
}
