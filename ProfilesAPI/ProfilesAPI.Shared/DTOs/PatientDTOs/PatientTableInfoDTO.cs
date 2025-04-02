namespace ProfilesAPI.Shared.DTOs.PatientDTOs;

public class PatientTableInfoDTO
{
    public Guid Id {get;set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; }
    public Guid PhotoId { get; set; }
    public string? Photo { get; set; }
}
