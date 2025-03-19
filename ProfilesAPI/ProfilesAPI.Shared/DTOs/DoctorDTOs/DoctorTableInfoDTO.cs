namespace ProfilesAPI.Shared.DTOs.DoctorDTOs;

public class DoctorTableInfoDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string WorkEmail { get; set; }
    public string Phone { get; set; }
    public Guid PhotoId { get; set; }
    public string? Photo { get; set; }
    public Guid OfficeId { get; set; }
    // public OfficeInfoDTO Office { get; set; }
    // public WorkStatusInfoDTO WorkStatus { get; set; }
    public Guid? WorkStatusId { get; set; }
    public ICollection<SpecializationsOfDoctorInfoDTO>? DoctorSpecializations { get; set; }
}
