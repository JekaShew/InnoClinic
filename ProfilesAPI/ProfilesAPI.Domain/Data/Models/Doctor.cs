namespace ProfilesAPI.Domain.Data.Models;

public class Doctor
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string? Address { get; set; }
    public string WorkEmail { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CareerStartDate { get; set; }
    public string Photo { get; set; }
    public Guid OfficeId { get; set; }

    public Guid? WorkStatusId { get; set; }
    public WorkStatus? WorkStatus { get; set; }

    public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
}
