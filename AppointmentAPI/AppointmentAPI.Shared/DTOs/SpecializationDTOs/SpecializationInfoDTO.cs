using AppointmentAPI.Shared.DTOs.DoctorSpecializationDTOs;

namespace AppointmentAPI.Shared.DTOs.SpecializationDTOs;

public class SpecializationInfoDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<DoctorSpecializationInfoDTO>? DoctorSpecializations { get; set; }
}
