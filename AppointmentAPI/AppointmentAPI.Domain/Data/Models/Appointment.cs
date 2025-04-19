namespace AppointmentAPI.Domain.Data.Models;

public class Appointment : BaseModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime AppintmentDate { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? OfficeId { get; set; }
    public Office? Office { get; set; }
    public Guid DoctorId  { get; set; }
    public Doctor? Doctor { get; set; } 

    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    public Guid SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }

    public ICollection<AppointmentResult>? AppointmentResults { get; set; } 
}

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}