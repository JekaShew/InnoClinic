namespace ProfilesAPI.Shared.DTOs.AdministratorDTOs;

public class AdministratorTableInfoDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string WorkEmail { get; set; }
    public Guid PhotoId { get; set; }
    public string? Photo { get; set; }
    // public OfficeInfoDTO Office { get; set; }
    // public WorkStatusInfoDTO WorkStatus { get; set; }
    public string? OfficeId { get; set; }
    public Guid? WorkStatusId { get; set; }
}
