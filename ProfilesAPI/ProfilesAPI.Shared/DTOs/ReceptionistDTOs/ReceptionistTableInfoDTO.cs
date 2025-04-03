namespace ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

public class ReceptionistTableInfoDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public Guid PhotoId { get; set; }
    public string? Photo { get; set; }
    // public OfficeInfoDTO Office { get; set; }
    // public WorkStatusInfoDTO WorkStatus { get; set; }
    public Guid? WorkStatusId { get; set; }
    public string? OfficeId { get; set; }
}
