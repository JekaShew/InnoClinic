using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Shared.DTOs.DoctorDTOs;

public class SpecializationsOfDoctorInfoDTO
{
    public Guid SpecializationId { get; set; }
    public SpecializationInfoDTO? Specialization { get; set; }
    public DateTime SpecialzationAchievementDate { get; set; }
    public string? Description { get; set; }
}
