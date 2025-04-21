using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Domain.IRepositories;

public interface IOfficeRepository : IGenericRepository<Office>
{
    public Task SoftDeleteAsync(Office office);
}
