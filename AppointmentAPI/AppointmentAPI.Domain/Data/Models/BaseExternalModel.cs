namespace AppointmentAPI.Domain.Data.Models;

public class BaseExternalModel : BaseModel
{
    public bool? IsDelete { get; set; } = false;
}
