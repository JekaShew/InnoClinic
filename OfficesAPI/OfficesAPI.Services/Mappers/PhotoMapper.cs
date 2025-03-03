using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Shared.DTOs.PhotoDTOs;
using Riok.Mapperly.Abstractions;

namespace OfficesAPI.Shared.Mappers;

[Mapper]
public static partial class PhotoMapper
{
    public static partial PhotoInfoDTO? PhotoToPhotoInfoDTO(Photo? photo);
}
