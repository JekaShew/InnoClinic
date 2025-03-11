using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Shared.DTOs.DoctorDTOs;

public class DoctorSpecializationInfoDTO
{
    public Guid DoctorId { get; set; }
    public DoctorInfoDTO Doctor { get; set; }
    public Guid SpecializationId { get; set; }
    public SpecializationInfoDTO Specialization { get; set; }
    public DateTime SpecialzationAchievementDate { get; set; }
    public string? WorkExperience { get; set; }
    public string? Description { get; set; }
}
