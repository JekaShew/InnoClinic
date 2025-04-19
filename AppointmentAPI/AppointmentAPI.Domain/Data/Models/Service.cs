namespace AppointmentAPI.Domain.Data.Models;

public class Service : BaseModel
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }   
    public ICollection<Appointment>? Appointments { get; set; }
}
