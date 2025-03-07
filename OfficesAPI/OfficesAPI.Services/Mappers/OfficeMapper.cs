using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Shared.DTOs.OfficeDTOs;
using Riok.Mapperly.Abstractions;
using SharpCompress.Common;

namespace OfficesAPI.Shared.Mappers;

[Mapper(AutoUserMappings = false)]
public static partial class OfficeMapper
{
    public static partial OfficeInfoDTO? OfficeToOfficeInfoDTO(Office? office);
    public static partial OfficeTableInfoDTO? OfficeToOfficeTableInfoDTO(Office? office);

    public static partial void UpdateOfficeFromOfficeForUpdateDTO(OfficeForUpdateDTO? dto, Office model);
    public static partial Office? OfficeForUpdateDTOToOffice(OfficeForUpdateDTO? officeForUpdateDTO);
    public static partial Office? OfficeForCreateDTOToOffice(OfficeForCreateDTO? officeForCreateDTO);
}
