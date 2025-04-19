using AppointmentAPI.Shared.DTOs.AppointmentDTOs;

namespace AppointmentAPI.Shared.DTOs.PatientDTOs;

public class PatientInfoDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<AppointmentTableInfoDTO>? Appointments { get; set; }
}
