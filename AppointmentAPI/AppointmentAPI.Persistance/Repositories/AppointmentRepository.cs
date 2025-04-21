using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
namespace AppointmentAPI.Persistance.Repositories;

public class AppointmentRepository : EFGenericRepository<Appointment>, IAppointmentRepository
{
    private readonly AppointmentsDBContext _appointmentDBContext;

    public AppointmentRepository(AppointmentsDBContext appointmentDBContext) : base(appointmentDBContext)
    {
        _appointmentDBContext = appointmentDBContext;
    }

    public async Task<IEnumerable<Appointment>> GetAllWithParametersAsync(AppointmentParameters? serviceParameters)
    {

    }
}

