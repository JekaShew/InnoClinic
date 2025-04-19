namespace AppointmentAPI.Domain.Data.Models;

public class Specialization : BaseModel
{
    public  string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<Appointment>? Appointments { get; set; }
    public ICollection<DoctorSpecialization>? DoctorSpecializations { get; set; }
}
