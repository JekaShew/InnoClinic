using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Shared.DTOs.PhotoDTOs;

namespace OfficesAPI.Services.Abstractions.Interfaces;

public interface IPhotoService
{
    public Task<ResponseMessage<PhotoInfoDTO>> AddPhototoOffice(string officeId, IFormFile formFile);
    public Task<ResponseMessage> DeleteOfficePhotoById(string officeId, string photoId);
    public Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotos();
    public Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotosOfOfficeById(string officeId);
    public Task<ResponseMessage<PhotoInfoDTO>> GetPhotoById(string photoId);
}
