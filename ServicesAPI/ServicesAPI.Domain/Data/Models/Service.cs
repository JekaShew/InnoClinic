namespace ServicesAPI.Domain.Data.Models;

public class Service : BaseModel
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid ServiceCategoryId { get; set; }
    public ServiceCategory? ServiceCategory { get; set; }
}
