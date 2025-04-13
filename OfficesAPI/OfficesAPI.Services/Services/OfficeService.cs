using CommonLibrary.RabbitMQEvents;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs.OfficeDTOs;
using OfficesAPI.Shared.Mappers;

namespace OfficesAPI.Services.Services;

public class OfficeService : IOfficeService
{
    private readonly IValidator<OfficeForCreateDTO> _officeForCreateValidator;
    private readonly IValidator<OfficeForUpdateDTO> _officeForUpdateValidator;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public OfficeService(
        IValidator<OfficeForCreateDTO> officeForCreateValidator,
        IValidator<OfficeForUpdateDTO> officeForUpdateValidator,
        IRepositoryManager repositoryManager,
        IPublishEndpoint publishEndpoint
        )
    {
        _officeForCreateValidator = officeForCreateValidator;
        _officeForUpdateValidator = officeForUpdateValidator;
        _repositoryManager = repositoryManager;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ResponseMessage<OfficeInfoDTO>> CreateOfficeAsync(OfficeForCreateDTO officeForCreateDTO, ICollection<IFormFile> files)
    {
        var validationResult = await _officeForCreateValidator.ValidateAsync(officeForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var office = OfficeMapper.OfficeForCreateDTOToOffice(officeForCreateDTO);
        office.Id = ObjectId.GenerateNewId().ToString();
        var officeId = office.Id;
        if (files is not null && files.Count != 0)
        {
            var photoList = new List<Photo>();
            foreach (var image in files)
            {
                var photo = new Photo();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.OpenReadStream().CopyTo(memoryStream);
                    photo.Url = Convert.ToBase64String(memoryStream.ToArray());
                }

                photo.Title = $"{officeForCreateDTO.City} {officeForCreateDTO.Street} {officeForCreateDTO.HouseNumber} {officeForCreateDTO.OfficeNumber}";
                photo.OfficeId = office.Id;
                _repositoryManager.Photo.AddPhoto(photo);
                photoList.Add(photo);
            }
            office.Photos = photoList;
        }

        _repositoryManager.Office.CreateOffice(office);
        await _repositoryManager.TransactionExecution(); 

        var officeCreatedEvent = OfficeMapper.OfficeToOfficeCreatedEvent(office);
        await _publishEndpoint.Publish(officeCreatedEvent);
        var officeInfoDTO = OfficeMapper.OfficeToOfficeInfoDTO(office);

        return new ResponseMessage<OfficeInfoDTO>(officeInfoDTO);
    }

    public async Task<ResponseMessage> DeleteOfficeByIdAsync(string officeId)
    {
        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage("No office found!", 404);
        }

        if(office.Photos is not null && office.Photos.Count != 0)
        {
            _repositoryManager.Office.DeleteOfficeById(officeId);
            _repositoryManager.Photo.DeletePhotosOfOfficeByOfficeId(officeId);
            await _repositoryManager.TransactionExecution();
        }

        _repositoryManager.Office.DeleteOfficeById(officeId);
        await _repositoryManager.SingleExecution();
        var officeDeletedEvent = OfficeMapper.OfficeToOfficeDeletedEvent(office);
        await _publishEndpoint.Publish(officeDeletedEvent);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<IEnumerable<OfficeTableInfoDTO>>> GetAllOfficesAsync()
    {
        var offices = await _repositoryManager.Office.GetAllOfficesAsync();
        var officeTableInfoDTOs = offices.Select(o => OfficeMapper.OfficeToOfficeTableInfoDTO(o));

        return new ResponseMessage<IEnumerable<OfficeTableInfoDTO>>(officeTableInfoDTOs);
    }

    public async Task<ResponseMessage<OfficeInfoDTO>> GetOfficeByIdAsync(string officeId)
    {
        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage<OfficeInfoDTO>("No Office found!", 404);
        }

        var officeInfoDTO = OfficeMapper.OfficeToOfficeInfoDTO(office);

        return new ResponseMessage<OfficeInfoDTO>(officeInfoDTO);
    }

    public async Task<ResponseMessage<OfficeInfoDTO>> UpdateOfficeInfoAsync(string officeId,[FromBody] OfficeForUpdateDTO officeForUpdateDTO)
    {
        var validationResult = await _officeForUpdateValidator.ValidateAsync(officeForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage<OfficeInfoDTO>("No Office found!", 404);
        }

        OfficeMapper.UpdateOfficeFromOfficeForUpdateDTO(officeForUpdateDTO, office);

        _repositoryManager.Office.UpdateOffice(office);
        await _repositoryManager.SingleExecution();
        var officeUpdatedEvent = OfficeMapper.OfficeToOfficeUpdatedEvent(office);
        await _publishEndpoint.Publish(officeUpdatedEvent);
        var officeInfoDTO = OfficeMapper.OfficeToOfficeInfoDTO(office); 

        return new ResponseMessage<OfficeInfoDTO>(officeInfoDTO);
    }

    public async Task<ResponseMessage> ChangeStatusOfOfficeByIdAsync(string officeId)
    {
        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage("No Office Found!", 404);
        }

        office.IsActive = !office.IsActive;

        _repositoryManager.Office.UpdateOffice(office);
        await _repositoryManager.SingleExecution();

        var officeUpdatedEvent = OfficeMapper.OfficeToOfficeUpdatedEvent(office);
        await _publishEndpoint.Publish(officeUpdatedEvent);

        return new ResponseMessage();
    }

    public async Task<IEnumerable<OfficeCheckConsistancyEvent>> GetAllOfficeCheckConsistancyEventsAsync()
    {
        var offices = await _repositoryManager.Office.GetAllOfficesAsync();
        var officeCheckConsistancyEvents = offices.Select(o => OfficeMapper.OfficeToOfficeCheckConsistancyEvent(o));

        return officeCheckConsistancyEvents;
    }
}
