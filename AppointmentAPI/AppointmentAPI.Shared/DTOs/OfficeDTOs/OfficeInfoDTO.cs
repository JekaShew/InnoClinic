using AppointmentAPI.Shared.DTOs.AppointmentDTOs;

namespace AppointmentAPI.Shared.DTOs.OfficeDTOs;

public class OfficeInfoDTO
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string? OfficeNumber { get; set; }
    public string? RegistryPhoneNumber { get; set; }

    public ICollection<AppointmentTableInfoDTO>? Appointments { get; set; }
}
