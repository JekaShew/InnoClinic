namespace ProfilesAPI.Shared.DTOs.DoctorDTOs;

public class DoctorSpecializationForCreateDTO
{
    public Guid SpecializationId { get; set; }
    public DateTime SpecialzationAchievementDate { get; set; }
    public string? Description { get; set; }
}
