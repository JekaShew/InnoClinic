namespace ProfilesAPI.Shared.DTOs.SpecializationDTOs;

public class SpecializationForCreateDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool? IsDelete { get; set; } = false;
}
