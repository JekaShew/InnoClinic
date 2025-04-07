namespace ServicesAPI.Shared.DTOs.SpecializationDTOs;

public class SpecializationForUpdateDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Guid>? ServiceCategories { get; set; }
}