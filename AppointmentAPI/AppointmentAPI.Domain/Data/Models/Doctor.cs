namespace AppointmentAPI.Domain.Data.Models;

public class Doctor : BaseModel
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }

    public ICollection<DoctorSpecialization>? DoctorSpecializations { get; set; }
    public ICollection<Appointment>? Appointments { get; set; } 
}
