using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using CommonLibrary.CommonService;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services;

public class UserStatusService : IUserStatusService
{
    private readonly IValidator<UserStatusForCreateDTO> _userStatusForCreateValidator;
    private readonly IValidator<UserStatusForUpdateDTO> _userStatusForUpdateValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    public UserStatusService(
            IRepositoryManager repositoryManager,
            ICommonService commonService,
            IValidator<UserStatusForCreateDTO> userStatusForCreateValidator,
            IValidator<UserStatusForUpdateDTO> userStatusForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _commonService = commonService;
        _userStatusForCreateValidator = userStatusForCreateValidator;
        _userStatusForUpdateValidator = userStatusForUpdateValidator;
    }
    public async Task<ResponseMessage> CreateUserStatusAsync(UserStatusForCreateDTO userStatusForCreateDTO)
    {
        var validationResult = await _userStatusForCreateValidator.ValidateAsync(userStatusForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        //var currentUserId = _commonService.GetCurrenUserId();
        //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        //if (!isAdmin)
        //{
        //    return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        //}

        var userStatus = UserStatusMapper.UserStatusForCreateDTOToUserStatus(userStatusForCreateDTO);
        await _repositoryManager.UserStatus.CreateUserStatusAsync(userStatus);
        await _repositoryManager.CommitAsync();
        
        return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);
    }

    public async Task<ResponseMessage> DeleteUserStatusByIdAsync(Guid userStatusId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userStatusId);
        if (userStatus is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }
            
        _repositoryManager.UserStatus.DeleteUserStatus(userStatus);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<UserStatusInfoDTO>>> GetAllUserStatusesAsync()
    {
        //var currentUserId = _commonService.GetCurrenUserId();
        //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        //if (!isAdmin)
        //{
        //    return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(MessageConstants.ForbiddenMessage, false);
        //}

        var userStatuses = await _repositoryManager.UserStatus.GetAllUserStatusesAsync();
        if (!userStatuses.Any())
        {
            return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(MessageConstants.NotFoundMessage, false);
        }
            
        var userStatusInfoDTOs = userStatuses.Select(us => UserStatusMapper.UserStatusToUserStatusInfoDTO(us));

        return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(MessageConstants.SuccessMessage, true,  userStatusInfoDTOs);
    }

    public async Task<ResponseMessage<UserStatusInfoDTO>> GetUserStatusByIdAsync(Guid userStatusId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage<UserStatusInfoDTO>(MessageConstants.ForbiddenMessage, false);
        }

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userStatusId);
        if (userStatus is null)
        {
            return new ResponseMessage<UserStatusInfoDTO>(MessageConstants.NotFoundMessage, false);
        }

        var userStatusInfoDTO = UserStatusMapper.UserStatusToUserStatusInfoDTO(userStatus);

        return new ResponseMessage<UserStatusInfoDTO>(MessageConstants.SuccessMessage,true, userStatusInfoDTO);
    }

    public async Task<ResponseMessage> UpdateUserStatusAsync(Guid userStatusId, UserStatusForUpdateDTO userStatusForUpdateDTO)
    {
        var validationResult = await _userStatusForUpdateValidator.ValidateAsync(userStatusForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        //var currentUserId = _commonService.GetCurrenUserId();
        //var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        //if (!isAdmin)
        //{
        //    return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        //}

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userStatusId, true);
        if (userStatus is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        UserStatusMapper.UpdateUserStatusFromUserStatusForUpdateDTO(userStatusForUpdateDTO, userStatus);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }
}
