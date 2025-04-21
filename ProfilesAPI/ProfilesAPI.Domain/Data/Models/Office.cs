using System.Xml.Linq;

namespace ProfilesAPI.Domain.Data.Models;

public class Office
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string? OfficeNumber { get; set; }
    public string RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public bool? IsDelete { get; set; } = false;

    public override bool Equals(object obj)
    {
        if (obj is Office other)
        {
            return Id == other.Id 
                && City == other.City
                && Street == other.Street
                && HouseNumber == other.HouseNumber
                && OfficeNumber == other.OfficeNumber
                && RegistryPhoneNumber == other.RegistryPhoneNumber
                && IsActive == other.IsActive;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, City, Street);
    }
}
