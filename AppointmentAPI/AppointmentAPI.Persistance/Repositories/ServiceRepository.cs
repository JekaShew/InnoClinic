using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class ServiceRepository : EFGenericRepository<Service>, IServiceRepository
{
    public ServiceRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
    }
}
