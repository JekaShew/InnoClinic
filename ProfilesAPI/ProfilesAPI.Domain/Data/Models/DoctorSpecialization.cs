namespace ProfilesAPI.Domain.Data.Models;

public class DoctorSpecialization
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public Guid SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
    public DateTime SpecialzationAchievementDate { get; set; }
    public string? WorkExperience { get; set; }
    public string? Description { get; set; }
}
