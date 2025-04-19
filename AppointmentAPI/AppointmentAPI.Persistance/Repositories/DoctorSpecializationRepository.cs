using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class DoctorSpecializationRepository : EFGenericRepository<DoctorSpecialization>, IDoctorSpecializationRepository
{
    public DoctorSpecializationRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
    }
}
