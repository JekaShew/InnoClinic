using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Repositories;

public class PatientRepository : EFGenericRepository<Patient>, IPatientRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;

    public PatientRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task SoftDeleteAsync(Patient patient)
    {
        var model = await _appointmentDBContext.Doctors.FirstOrDefaultAsync(d => d.Id.Equals(patient.Id));
        model.IsDelete = true;
        await _appointmentDBContext.SaveChangesAsync();
    }
}
