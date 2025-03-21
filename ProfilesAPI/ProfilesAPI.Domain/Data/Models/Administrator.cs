namespace ProfilesAPI.Domain.Data.Models;

public class Administrator : BaseProfileInformation
{
    public string WorkEmail { get; set; }
    public DateTime CareerStartDate { get; set; }
    public string OfficeId { get; set; }
    public Guid? WorkStatusId { get; set; }
    public WorkStatus? WorkStatus { get; set; }
}
