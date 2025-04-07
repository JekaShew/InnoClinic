namespace ServicesAPI.Domain.Data.Models;

public class Specialization : BaseModel
{
    public string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<ServiceCategorySpecialization>? ServiceCategorySpecializations { get; set; }
}
