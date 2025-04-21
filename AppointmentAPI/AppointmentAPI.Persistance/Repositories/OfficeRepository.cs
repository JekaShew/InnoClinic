using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Repositories;

public class OfficeRepository : EFGenericRepository<Office>, IOfficeRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;

    public OfficeRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task SoftDeleteAsync(Office office)
    {
        var model = await _appointmentDBContext.Doctors.FirstOrDefaultAsync(d => d.Id.Equals(office.Id));
        model.IsDelete = true;
        await _appointmentDBContext.SaveChangesAsync();
    }
}
