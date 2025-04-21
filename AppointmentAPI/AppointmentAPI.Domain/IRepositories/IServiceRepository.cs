using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Domain.IRepositories;

public interface IServiceRepository : IGenericRepository<Service>
{
    public Task SoftDeleteAsync(Service service);
}
