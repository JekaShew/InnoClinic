namespace ProfilesAPI.Domain.Data.Models;

public class WorkStatus
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Doctor>? Doctors { get; set; }
    public ICollection<Patient>? Patients { get; set; }
    public ICollection<Receptionist>? Receptionists { get; set; }
    public ICollection<Administrator>? Administrators { get; set; }
}
