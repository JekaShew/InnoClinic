using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs.PhotoDTOs;
using OfficesAPI.Shared.Mappers;

namespace OfficesAPI.Services.Services;

public class PhotoService : IPhotoService
{
    private readonly IRepositoryManager _repositoryManager;

    public PhotoService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<ResponseMessage<string>> AddPhototoOffice(string officeId, IFormFile formFile)
    {
        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if(office is null)
        {
            return new ResponseMessage<string>("No Office found!", 404);
        }
        var photo = new Photo();

        using (MemoryStream memoryStream = new MemoryStream())
        {
            formFile.OpenReadStream().CopyTo(memoryStream);
            photo.Url = Convert.ToBase64String(memoryStream.ToArray());
        }
        photo.Title = $"{office.City} {office.Street} {office.HouseNumber} {office.OfficeNumber}";
        photo.OfficeId = office.Id;
        _repositoryManager.Photo.AddPhoto(photo);
        office.Photos.Add(photo);
        _repositoryManager.Office.UpdateOffice(office);

        await _repositoryManager.TransactionExecution();
        var photoId = photo.Id;

        return new ResponseMessage<string>(photoId);
    }
    public async Task<ResponseMessage> DeleteOfficePhotoById(string officeId,string photoId)
    {
        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage("No Office Found!", 404);
        }

        var photo = await _repositoryManager.Photo.GetPhotoById(photoId);
        if (photo is null)
        {
            return new ResponseMessage("No Photo Found!", 404);
        }

        _repositoryManager.Photo.DeletePhotoById(photoId);
        office.Photos.Remove(photo);
        _repositoryManager.Office.UpdateOffice(office);
        await _repositoryManager.TransactionExecution();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotos()
    {
        var photos = await _repositoryManager.Photo.GetAllPhotos();
        var photoDTOs = photos.Select(p => PhotoMapper.PhotoToPhotoInfoDTO(p));

        return new ResponseMessage<IEnumerable<PhotoInfoDTO>>(photoDTOs);
    }

    public async Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotosOfOfficeById(string officeId)
    {
        var photos = await _repositoryManager.Photo.GetPhotoListByFilter(x => x.OfficeId.Equals(officeId));
        var photoDTOs = photos.Select(p => PhotoMapper.PhotoToPhotoInfoDTO(p));

        return new ResponseMessage<IEnumerable<PhotoInfoDTO>>(photoDTOs);
    }

    public async Task<ResponseMessage<PhotoInfoDTO>> GetPhotoById(string photoId)
    {
        var photo = await _repositoryManager.Photo.GetPhotoById(photoId);
        if(photo is null)
        {
            return new ResponseMessage<PhotoInfoDTO>("No Photo Found!", 404);
        }

        var photoDTO = PhotoMapper.PhotoToPhotoInfoDTO(photo);

        return new ResponseMessage<PhotoInfoDTO>(photoDTO);
    }
}
