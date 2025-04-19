using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AppointmentAPI.Shared.DTOs.DoctorSpecializationDTOs;

namespace AppointmentAPI.Shared.DTOs.DoctorDTOs;

public class DoctorInfoDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }

    public ICollection<DoctorSpecializationInfoDTO>? DoctorSpecializations { get; set; }
    public ICollection<AppointmentInfoDTO>? Appointments { get; set; }
}
