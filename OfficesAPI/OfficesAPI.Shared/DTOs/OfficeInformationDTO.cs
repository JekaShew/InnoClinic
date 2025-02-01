namespace OfficesAPI.Shared.DTOs
{
    public class OfficeInformationDTO
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public List<PhotoDTO> PhotoDTOs { get; set; }   
    }
}
