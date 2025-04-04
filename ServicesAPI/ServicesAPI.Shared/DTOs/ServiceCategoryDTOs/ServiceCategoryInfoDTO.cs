using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

public class ServiceCategoryInfoDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<ServiceInfoDTO>? Services { get; set; }
}
