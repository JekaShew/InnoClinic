using AppointmentAPI.Shared.DTOs.AppointmentDTOs;

namespace AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;

public class AppointmentResultTableInfoDTO
{
    public string? Title { get; set; }
    public DateTime ResultDate { get; set; }
    public string PatientFullName { get; set; }
    public string ServiceTitle { get; set; }
    public string OfficeAddress { get; set; }
    public Guid AppointmentId { get; set; }
    public AppointmentTableInfoDTO? Appointment { get; set; }
}