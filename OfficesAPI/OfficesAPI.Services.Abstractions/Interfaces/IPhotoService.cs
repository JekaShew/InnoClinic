using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Shared.DTOs.PhotoDTOs;

namespace OfficesAPI.Services.Abstractions.Interfaces;

public interface IPhotoService
{
    public Task<ResponseMessage<PhotoInfoDTO>> AddPhotoToOffice(Guid officeId, IFormFile formFile);
    public Task<ResponseMessage> DeleteOfficePhotoById(Guid officeId, Guid photoId);
    public Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotos();
    public Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotosOfOfficeById(Guid officeId);
    public Task<ResponseMessage<PhotoInfoDTO>> GetPhotoById(Guid photoId);
}
