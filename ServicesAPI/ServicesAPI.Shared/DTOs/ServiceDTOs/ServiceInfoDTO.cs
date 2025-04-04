using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Shared.DTOs.ServiceDTOs;

public class ServiceInfoDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid ServiceCategoryId { get; set; }
    public ServiceCategoryInfoDTO? ServiceCategory { get; set; }
}
