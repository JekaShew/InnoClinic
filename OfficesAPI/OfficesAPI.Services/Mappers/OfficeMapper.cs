using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Shared.DTOs;
using Riok.Mapperly.Abstractions;

namespace OfficesAPI.Shared.Mappers
{
    [Mapper]
    public static partial class OfficeMapper
    {
        //[MapProperty(nameof(OfficeInformationDTO.Address), nameof(Office.Street), Use = nameof(CollectAddress))]
        public static partial OfficeInformationDTO? OfficeToOfficeInformationDTO(Office? office);

        //[MapProperty(nameof(OfficeTableInformationDTO.Address), nameof(Office), Use = nameof(CollectAddress))]
        public static partial OfficeTableInformationDTO? OfficeToOfficeTableInformationDTO(Office? office);
        
        public static partial Office? OfficeDTOToOffice(OfficeDTO? officeDTO);
        public static partial OfficeDTO? OfficeToOfficeDTO(Office? office);


        [UserMapping]
        private static string CollectAddress( Office office ) =>
                    $"C. {office.City}, St. {office.Street} {office.HouseNumber}, Office {office.OfficeNumber}";
    }
}
