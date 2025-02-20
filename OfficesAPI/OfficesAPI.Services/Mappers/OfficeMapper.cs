using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Shared.DTOs.OfficeDTOs;
using Riok.Mapperly.Abstractions;
using SharpCompress.Common;

namespace OfficesAPI.Shared.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public static partial class OfficeMapper
    {
        //[MapProperty(nameof(OfficeInfoDTO.Address), MapWith = nameof(CollectAddress))]

        //[MapProperty(nameof(OfficeInfoDTO.Address), expression: "$\"{source.Field1} {source.Field2} {source.Field3}\"")]
        
        //[MapProperty( (nameof(Office.City), nameof(Office.Street), nameof(Office.HouseNumber), nameof(Office.OfficeNumber)),
        //    nameof(OfficeInfoDTO.Address),
        //   CollectAddress() = nameof(CollectAddress))]
        public static partial OfficeInfoDTO? OfficeToOfficeInfoDTO(Office? office);

        //[MapProperty(nameof(OfficeTableInformationDTO.Address), nameof(Office), Use = nameof(CollectAddress))]
        public static partial OfficeTableInfoDTO? OfficeToOfficeTableInfoDTO(Office? office);

       
        public static partial Office? OfficeForUpdateDTOToOffice(OfficeForUpdateDTO? officeForUpdateDTO);
        public static partial Office? OfficeForCreateDTOToOffice(OfficeForCreateDTO? officeForCreateDTO);

        //[UserMapping]
        //private static string CollectAddress(Office office)
        //{
        //    OfficeInfoDTO. $"C. {office.City}, St. {office.Street} {office.HouseNumber}, Office {office.OfficeNumber}";
        //

        //[UserMapping]
        //private static string CollectAddress(
        //        string city,
        //        string street,
        //        string houseNumber,
        //        string? officeNumber = "") =>
        //            $"C. {city}, St. {street} {houseNumber}, Office {officeNumber}";
    }
}
