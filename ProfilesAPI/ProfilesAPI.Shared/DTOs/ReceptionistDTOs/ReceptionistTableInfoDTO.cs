namespace ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

public class ReceptionistTableInfoDTO
{
    // ToUpdate necessary data
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string WorkEmail { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CareerStartDate { get; set; }
    public string Photo { get; set; }
    public Guid? WorkStatusId { get; set; }
    public Guid OfficeId { get; set; }
}
