using AppointmentAPI.Shared.DTOs.AppointmentDTOs;

namespace AppointmentAPI.Shared.DTOs.ServiceDTOs;

public class ServiceInfoDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public ICollection<AppointmentTableInfoDTO>? Appointments { get; set; }
}
