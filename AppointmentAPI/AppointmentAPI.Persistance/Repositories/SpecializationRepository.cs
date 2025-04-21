using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Repositories;

public class SpecializationRepository : EFGenericRepository<Specialization>, ISpecializationRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;

    public SpecializationRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task SoftDeleteAsync(Specialization specialization)
    {
        var model = await _appointmentDBContext.Doctors.FirstOrDefaultAsync(d => d.Id.Equals(specialization.Id));
        model.IsDelete = true;
        await _appointmentDBContext.SaveChangesAsync();
    }
}
