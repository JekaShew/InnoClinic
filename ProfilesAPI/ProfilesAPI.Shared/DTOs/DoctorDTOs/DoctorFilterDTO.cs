namespace ProfilesAPI.Shared.DTOs.DoctorDTOs;

public class DoctorFilterDTO
{
    public ICollection<Guid>? SpecializationIds{ get; set; }
    public ICollection<string>? OfficeIds { get; set; }
    public string? QueryString { get; set; }
}
