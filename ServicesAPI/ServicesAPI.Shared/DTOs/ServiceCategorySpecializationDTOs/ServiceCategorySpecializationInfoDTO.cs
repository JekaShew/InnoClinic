using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

public class ServiceCategorySpecializationInfoDTO
{
    public Guid ServiceCategoryId { get; set; }
    public Guid SpecializationId { get; set; }
    public string? Description { get; set; }

    public ServiceCategoryTableInfoDTO? ServiceCategory { get; set; }
    public SpecializationTableInfoDTO? Specialization { get; set; }
}
