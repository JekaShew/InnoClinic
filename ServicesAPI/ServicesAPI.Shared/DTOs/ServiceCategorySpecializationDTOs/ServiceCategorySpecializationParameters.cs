namespace ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

public class ServiceCategorySpecializationParameters : RequestParameters
{
    public string? ServiceCategorySearchString { get; set; }
    public string? SpecializationSearchString { get; set; }
    public ICollection<Guid>? ServiceCategories{ get; set; }
    public ICollection<Guid>? Specializations{ get; set; }
}
