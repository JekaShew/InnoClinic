using AppointmentAPI.Shared.DTOs.DoctorDTOs;
using AppointmentAPI.Shared.DTOs.SpecializationDTOs;

namespace AppointmentAPI.Shared.DTOs.DoctorSpecializationDTOs;

public class DoctorSpecializationInfoDTO
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public DoctorInfoDTO? Doctor { get; set; }
    public Guid SpecializationId { get; set; }
    public SpecializationInfoDTO? Specialization { get; set; }
}
