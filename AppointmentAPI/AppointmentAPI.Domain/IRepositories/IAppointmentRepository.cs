using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;

namespace AppointmentAPI.Domain.IRepositories;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    public Task<IEnumerable<Appointment>> GetAllWithParametersAsync(AppointmentParameters? appoinmentParameters);
}
