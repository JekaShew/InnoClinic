using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class DoctorRepository : EFGenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
    }
}
