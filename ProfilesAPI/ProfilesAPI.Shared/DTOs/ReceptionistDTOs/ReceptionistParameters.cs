namespace ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

public class ReceptionistParameters : RequestParameters
{
    public ICollection<Guid>? Offices { get; set; }
    public string? SearchString { get; set; }
}
