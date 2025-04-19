using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;

namespace AppointmentAPI.Persistance.Repositories;

public class AppointmentResultRepository : EFGenericRepository<AppointmentResult>, IAppointmentResultRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;

    public AppointmentResultRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task<IEnumerable<AppointmentResult>> GetAllWithParametersAsync(AppointmentResultParameters? serviceParameters)
    {

    }
}
