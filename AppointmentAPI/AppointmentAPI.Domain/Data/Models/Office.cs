namespace AppointmentAPI.Domain.Data.Models;

public class Office : BaseExternalModel
{
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string? OfficeNumber { get; set; }
    public string? RegistryPhoneNumber { get; set; }

    public ICollection<Appointment>? Appointments { get; set; }
}
