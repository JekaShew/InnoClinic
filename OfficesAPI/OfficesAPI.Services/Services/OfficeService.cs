using CommonLibrary.CommonService;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.Constnts;
using OfficesAPI.Shared.DTOs;
using OfficesAPI.Shared.Mappers;

namespace OfficesAPI.Services.Services;

public class OfficeService : IOfficeService
{
    private readonly IValidator<OfficeForCreateDTO> _officeForCreateValidator;
    private readonly IValidator<OfficeForUpdateDTO> _officeForUpdateValidator;
    private readonly ICommonService _commonService;
    private readonly IRepositoryManager _repositoryManager;


    public OfficeService(
        IValidator<OfficeForCreateDTO> officeForCreateValidator,
        IValidator<OfficeForUpdateDTO> officeForUpdateValidator,
        ICommonService commonService,
        IRepositoryManager repositoryManager)
    {
        _officeForCreateValidator = officeForCreateValidator;
        _officeForUpdateValidator = officeForUpdateValidator;
        _commonService = commonService;
        _repositoryManager = repositoryManager;
    }
    public async Task<ResponseMessage> CreateOfficeAsync(OfficeForCreateDTO officeForCreateDTO)
    {
        var validationResult = await _officeForCreateValidator.ValidateAsync(officeForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserId = _commonService.GetCurrentUserId();
        // ask AuthorizationAPI if the current user is an admin
        //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        //if (!isAdmin)
        //{
        //    return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        //}

        var office = OfficeMapper.OfficeForCreateDTOToOffice(officeForCreateDTO);

        _repositoryManager.Office.CreateOffice(office);
        await _repositoryManager.SingleExecution();

        return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);            
    }

    public async Task<ResponseMessage> DeleteOfficeByIdAsync(string officeId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        // ask AuthorizationAPI if the current user is an admin
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

        _repositoryManager.Office.DeleteOfficeById(officeId);
        await _repositoryManager.SingleExecution();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<OfficeTableInfoDTO>>> GetAllOfficesAsync()
    {           
        var offices = await _repositoryManager.Office.GetAllOfficesAsync();
        if(!offices.Any())
        {
            return new ResponseMessage<IEnumerable<OfficeTableInfoDTO>>(MessageConstants.NotFoundMessage, false);
        }

        var officeTableInfoDTOs = offices.Select(o => OfficeMapper.OfficeToOfficeTableInfoDTO(o));

        return new ResponseMessage<IEnumerable<OfficeTableInfoDTO>>(MessageConstants.SuccessMessage, true, officeTableInfoDTOs);
    }

    public async Task<ResponseMessage<OfficeInfoDTO>> GetOfficeByIdAsync(string officeId)
    {
        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage<OfficeInfoDTO>(MessageConstants.NotFoundMessage, false);
        }

        var officeInfoDTO = OfficeMapper.OfficeToOfficeInfoDTO(office);

        return new ResponseMessage<OfficeInfoDTO>(MessageConstants.SuccessMessage, true, officeInfoDTO);
    }

    // добавить доп логику по работе с фотографиями(апдейтить фотографии через обновление коллекции монги??)
    public async Task<ResponseMessage> UpdateOfficeAsync(string officeId, OfficeForUpdateDTO officeForUpdateDTO)
    {
        var validationResult = await _officeForUpdateValidator.ValidateAsync(officeForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserId = _commonService.GetCurrentUserId();
        //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        //if (!isAdmin)
        //{
        //    return new ResponseMessage<RoleInfoDTO>(MessageConstants.ForbiddenMessage, false);
        //}

        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if(office is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        _repositoryManager.Office.UpdateOffice(office);
        await _repositoryManager.SingleExecution();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }

    public async Task<ResponseMessage> ChangeStatusOfOfficeByIdAsync(string officeId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        //if (!isAdmin)
        //{
        //    return new ResponseMessage<RoleInfoDTO>(MessageConstants.ForbiddenMessage, false);
        //}

        var office = await _repositoryManager.Office.GetOfficeByIdAsync(officeId);
        if (office is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        office.IsActive = !office.IsActive;

        _repositoryManager.Office.UpdateOffice(office);
        await _repositoryManager.SingleExecution();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }
}
