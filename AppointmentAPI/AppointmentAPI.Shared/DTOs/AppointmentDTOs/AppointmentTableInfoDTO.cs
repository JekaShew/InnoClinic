using AppointmentAPI.Shared.DTOs.DoctorDTOs;
using AppointmentAPI.Shared.DTOs.ServiceDTOs;

namespace AppointmentAPI.Shared.DTOs.AppointmentDTOs;

public class AppointmentTableInfoDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime AppintmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public Guid DoctorId { get; set; }
    public DoctorInfoDTO? Doctor { get; set; }
    public Guid ServiceId { get; set; }
    public ServiceInfoDTO Service { get; set; }
}

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}


