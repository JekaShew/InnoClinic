namespace ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

public class ServiceCategoryForUpdateDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Guid>? Specialziations { get; set; }
}
