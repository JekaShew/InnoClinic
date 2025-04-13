namespace ProfilesAPI.Domain.Data.Models;

public class Specialization
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool? IsDelete { get; set; } = false;

    public ICollection<DoctorSpecialization>? DoctorSpecializations { get; set; }
}
