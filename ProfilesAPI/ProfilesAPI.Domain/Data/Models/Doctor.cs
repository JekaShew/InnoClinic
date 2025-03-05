namespace ProfilesAPI.Domain.Data.Models;

public class Doctor : BaseProfileInformation
{
    public Guid UserId { get; set; }
    public string WorkEmail { get; set; }
    public DateTime CareerStartDate { get; set; }
    public Guid OfficeId { get; set; }

    public Guid? WorkStatusId { get; set; }
    public WorkStatus? WorkStatus { get; set; }

    public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
}
