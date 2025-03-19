namespace ProfilesAPI.Shared.DTOs.AdministratorDTOs;

public class AdministratorParameters : RequestParameters
{
    public ICollection<string>? Offices { get; set; }
    public string? SearchString { get; set; }
}
