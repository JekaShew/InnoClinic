using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Repositories;

public class DoctorRepository : EFGenericRepository<Doctor>, IDoctorRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;
    public DoctorRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task SoftDeleteAsync(Doctor doctor)
    {
        var model = await _appointmentDBContext.Doctors.FirstOrDefaultAsync(d => d.Id.Equals(doctor.Id));
        model.IsDelete = true;
        await _appointmentDBContext.SaveChangesAsync();
    }
}
