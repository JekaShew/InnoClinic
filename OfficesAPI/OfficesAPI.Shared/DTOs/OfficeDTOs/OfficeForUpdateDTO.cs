namespace OfficesAPI.Shared.DTOs.OfficeDTOs;

public class OfficeForUpdateDTO
{
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string? OfficeNumber { get; set; }
    public string? RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public ICollection<string>? Photos { get; set; }
    //public List<PhotoDTO> Photos { get; set; }
}
