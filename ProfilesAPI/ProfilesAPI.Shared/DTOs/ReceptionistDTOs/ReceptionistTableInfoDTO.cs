namespace ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

public class ReceptionistTableInfoDTO
{
    // ToUpdate necessary data
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public Guid? PhotoId { get; set; }
    public FileResponse? Photo { get; set; }
    public Guid? WorkStatusId { get; set; }
    public Guid OfficeId { get; set; }
}
