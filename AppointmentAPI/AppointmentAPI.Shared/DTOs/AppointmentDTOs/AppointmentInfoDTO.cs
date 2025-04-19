using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AppointmentAPI.Shared.DTOs.DoctorDTOs;
using AppointmentAPI.Shared.DTOs.OfficeDTOs;
using AppointmentAPI.Shared.DTOs.PatientDTOs;
using AppointmentAPI.Shared.DTOs.ServiceDTOs;
using AppointmentAPI.Shared.DTOs.SpecializationDTOs;

namespace AppointmentAPI.Shared.DTOs.AppointmentDTOs;

public class AppointmentInfoDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime AppintmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? OfficeId { get; set; }
    public OfficeInfoDTO? Office { get; set; }
    public Guid DoctorId { get; set; }
    public DoctorInfoDTO? Doctor { get; set; }

    public Guid PatientId { get; set; }
    public PatientInfoDTO? Patient { get; set; }
    public Guid ServiceId { get; set; }
    public ServiceInfoDTO Service { get; set; }
    public Guid SpecializationId { get; set; }
    public SpecializationInfoDTO? Specialization { get; set; }

    public ICollection<AppointmentResultTableInfoDTO>? AppointmentResults { get; set; }
}
