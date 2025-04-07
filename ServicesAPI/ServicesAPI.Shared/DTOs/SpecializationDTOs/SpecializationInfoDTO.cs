using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Shared.DTOs.SpecializationDTOs;

public class SpecializationInfoDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<ServiceCategoryInfoDTO>? ServiceCategorySpecializations { get; set; }
}
