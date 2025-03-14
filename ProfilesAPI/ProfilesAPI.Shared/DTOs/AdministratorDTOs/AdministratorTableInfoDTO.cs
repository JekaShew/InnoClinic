namespace ProfilesAPI.Shared.DTOs.AdministratorDTOs;

public class AdministratorTableInfoDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string WorkEmail { get; set; }
    public Guid? PhotoId { get; set; }
    public FileResponse? Photo { get; set; }
    public Guid OfficeId { get; set; }
    public Guid? WorkStatusId { get; set; }
}
