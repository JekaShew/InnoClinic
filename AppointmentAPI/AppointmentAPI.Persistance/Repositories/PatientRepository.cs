using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class PatientRepository : EFGenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
    }
}
