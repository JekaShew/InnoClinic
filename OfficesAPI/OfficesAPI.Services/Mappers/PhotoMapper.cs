using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Shared.DTOs;
using Riok.Mapperly.Abstractions;

namespace OfficesAPI.Shared.Mappers
{
    [Mapper]
    public static partial class PhotoMapper
    {

        public static partial Photo? PhotoDTOToPhoto(PhotoDTO? photoDTO);
        public static partial PhotoDTO? PhotoToPhotoDTO(Photo? photo);
    }
}
