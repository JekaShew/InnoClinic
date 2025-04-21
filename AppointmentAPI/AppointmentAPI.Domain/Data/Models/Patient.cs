namespace AppointmentAPI.Domain.Data.Models;

public class Patient : BaseExternalModel
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }

    public ICollection<Appointment>? Appointments { get; set; }
}
