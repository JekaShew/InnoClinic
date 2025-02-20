namespace OfficesAPI.Shared.DTOs
{
    public class OfficeInfoDTO
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public bool IsActive { get; set;}
        public string RegistryPhoneNumber { get; set; }
        public List<PhotoDTO> PhotoDTOs { get; set; }   
    }
}
