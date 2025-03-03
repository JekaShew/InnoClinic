using CommonLibrary.CommonService;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.Constnts;
using OfficesAPI.Shared.DTOs.PhotoDTOs;
using OfficesAPI.Shared.Mappers;

namespace OfficesAPI.Services.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ICommonService _commonService;
        private readonly IRepositoryManager _repositoryManager;

        public PhotoService(IRepositoryManager repositoryManager, ICommonService commonService)
        {
            _repositoryManager = repositoryManager;
            _commonService = commonService;
        }
        public async Task<ResponseMessage> AddPhototoOffice(string officeId, IFormFile formFile)
        {
            // ask AuthorizationAPI if the current user is an admin
            var currentUserId = _commonService.GetCurrentUserId();
            //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
            //if (!isAdmin)
            //{
            //    return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
            //}

            var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
            if(office is null)
            {
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);
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

            _repositoryManager.TransactionExecution();

            return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);
        }
        public async Task<ResponseMessage> DeleteOfficePhotoById(string officeId,string photoId)
        {
            // ask AuthorizationAPI if the current user is an admin
            var currentUserId = _commonService.GetCurrentUserId();
            //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
            //if (!isAdmin)
            //{
            //    return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
            //}

            var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
            if (office is null)
            {
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);
            }

            var photo = await _repositoryManager.Photo.GetPhotoById(photoId);
            if (photo is null)
            {
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);
            }

            _repositoryManager.Photo.DeletePhotoById(photoId);
            office.Photos.Remove(photo);
            _repositoryManager.Office.UpdateOffice(office);
            await _repositoryManager.TransactionExecution();

            return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
        }

        public async Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotos()
        {
            // ask AuthorizationAPI if the current user is an admin
            var currentUserId = _commonService.GetCurrentUserId();
            //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
            //if (!isAdmin)
            //{
            //    return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
            //}

            var photos = await _repositoryManager.Photo.GetAllPhotos();
            if(!photos.Any())
            {
                return new ResponseMessage<IEnumerable<PhotoInfoDTO>>(MessageConstants.NotFoundMessage, false);
            }

            var photoDTOs = photos.Select(p => PhotoMapper.PhotoToPhotoInfoDTO(p));

            return new ResponseMessage<IEnumerable<PhotoInfoDTO>>(MessageConstants.SuccessMessage, true, photoDTOs);
        }

        public async Task<ResponseMessage<IEnumerable<PhotoInfoDTO>>> GetAllPhotosOfOfficeById(string officeId)
        {
            var filter = Builders<Photo>.Filter.Eq(o => o.OfficeId, officeId);
            var photos = await _repositoryManager.Photo.GetPhotoListWithFilter(filter);
            if(!photos.Any())
            {
                return new ResponseMessage<IEnumerable<PhotoInfoDTO>>(MessageConstants.NotFoundMessage, false);
            }

            var photoDTOs = photos.Select(p => PhotoMapper.PhotoToPhotoInfoDTO(p));

            return new ResponseMessage<IEnumerable<PhotoInfoDTO>>(MessageConstants.SuccessMessage, true, photoDTOs);
        }

        public async Task<ResponseMessage<PhotoInfoDTO>> GetPhotoById(string photoId)
        {
            var photo = await _repositoryManager.Photo.GetPhotoById(photoId);
            if(photo is null)
            {
                return new ResponseMessage<PhotoInfoDTO>(MessageConstants.NotFoundMessage, false);
            }

            var photoDTO = PhotoMapper.PhotoToPhotoInfoDTO(photo);

            return new ResponseMessage<PhotoInfoDTO>(MessageConstants.SuccessMessage, true, photoDTO);
        }
    }
}
