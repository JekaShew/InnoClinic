namespace AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;

public class AppointmentResultForUpdateDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Complaints { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Conclusion { get; set; }
    public string? Recommendations { get; set; }

    public Guid AppointmentId { get; set; }
}
