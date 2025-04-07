namespace ServicesAPI.Shared.DTOs.SpecializationDTOs;

public class SpecializationForCreateDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Guid>? ServiceCategories { get; set; }
}
