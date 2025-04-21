using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Repositories;

public class ServiceRepository : EFGenericRepository<Service>, IServiceRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;

    public ServiceRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task SoftDeleteAsync(Service service)
    {
        var model = await _appointmentDBContext.Doctors.FirstOrDefaultAsync(d => d.Id.Equals(service.Id));
        model.IsDelete = true;
        await _appointmentDBContext.SaveChangesAsync();
    }
}
