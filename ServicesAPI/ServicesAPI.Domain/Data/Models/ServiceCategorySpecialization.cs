namespace ServicesAPI.Domain.Data.Models;

public class ServiceCategorySpecialization : BaseModel
{
    public Guid ServiceCategoryId { get; set; }
    public Guid SpecializationId { get; set; }
    public string? Description { get; set; }

    public ServiceCategory? ServiceCategory { get; set; }
    public Specialization? Specialization { get; set; }
}

