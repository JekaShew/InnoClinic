namespace ServicesAPI.Domain.Data.Models;

public class ServiceCategory
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<Service>? Services { get; set; }
    public ICollection<ServiceCategorySpecialization>? ServiceCategorySpecializations { get; set; }
}
