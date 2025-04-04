namespace ServicesAPI.Shared.DTOs.ServiceDTOs;

public class ServiceForCreateDTO
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid ServiceCategoryId { get; set; }
}
