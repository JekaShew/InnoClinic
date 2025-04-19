namespace AppointmentAPI.Shared.DTOs.AppointmentDTOs;

public class AppointmentForUpdateDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime AppintmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? OfficeId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid SpecializationId { get; set; }
}
