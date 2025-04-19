using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;

namespace AppointmentAPI.Domain.IRepositories;

public interface IAppointmentResultRepository : IGenericRepository<AppointmentResult>
{
    public Task<IEnumerable<AppointmentResult>> GetAllWithParametersAsync(AppointmentResultParameters? appointmentResultParameters);
}
