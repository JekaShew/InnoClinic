namespace ServicesAPI.Shared.DTOs.ServiceDTOs;

public class ServiceParameters : RequestParameters
{
    public string? SearchString { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public ICollection<Guid>? ServiceCategories { get; set; }

}
