using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class SpecializationRepository : EFGenericRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
    }
}
