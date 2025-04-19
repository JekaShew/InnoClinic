namespace AppointmentAPI.Domain.Data.Models;

public class DoctorSpecialization : BaseModel
{
    public Guid DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public Guid SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }
}
