namespace ProfilesAPI.Shared.DTOs.DoctorDTOs;

public class DoctorParameters : RequestParameters
{
    public ICollection<Guid>? Specializations { get; set; }
    public ICollection<string>? Offices { get; set; }

    public string? SearchString { get; set; }
}
