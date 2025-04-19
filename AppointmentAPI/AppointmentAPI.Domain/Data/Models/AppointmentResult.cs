namespace AppointmentAPI.Domain.Data.Models;

public class AppointmentResult : BaseModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime ResultDate { get; set; }
    public string PatientFullName { get; set; }
    public DateTime PatientBirthDate { get; set; }
    public string DoctorFullName { get; set; }
    public string ServiceTitle { get; set; }
    public string SpecializationTitle { get; set; }
    public string OfficeAddress { get; set; }

    public string? Complaints { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Conclusion { get; set; }
    public string? Recommendations { get; set; }
    public string? ResultUrl { get; set; }

    public Guid AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
}
