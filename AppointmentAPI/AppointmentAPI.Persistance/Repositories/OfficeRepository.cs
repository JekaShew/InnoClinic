using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class OfficeRepository : EFGenericRepository<Office>, IOfficeRepository
{
    // note that officeId is not a guid but just string BsonId and need to be serialized to Guid? 
    public OfficeRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
    }
}
