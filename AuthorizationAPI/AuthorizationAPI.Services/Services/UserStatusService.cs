using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
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
    public async Task<ResponseMessage<UserStatusInfoDTO>> CreateUserStatusAsync(UserStatusForCreateDTO userStatusForCreateDTO)
    {
        var validationResult = await _userStatusForCreateValidator.ValidateAsync(userStatusForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var userStatus = UserStatusMapper.UserStatusForCreateDTOToUserStatus(userStatusForCreateDTO);
        await _repositoryManager.UserStatus.CreateUserStatusAsync(userStatus);
        await _repositoryManager.CommitAsync();
        var userStatusInfoDTO = UserStatusMapper.UserStatusToUserStatusInfoDTO(userStatus);

        return new ResponseMessage<UserStatusInfoDTO>(userStatusInfoDTO);
    }

    public async Task<ResponseMessage> DeleteUserStatusByIdAsync(Guid userStatusId)
    {
        var currentUserId = _commonService.GetCurrentUserInfo();

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userStatusId);
        if (userStatus is null)
        {
            return new ResponseMessage("No User Status Found!", 404);
        }
            
        _repositoryManager.UserStatus.DeleteUserStatus(userStatus);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<IEnumerable<UserStatusInfoDTO>>> GetAllUserStatusesAsync()
    {
        var userStatuses = await _repositoryManager.UserStatus.GetAllUserStatusesAsync();            
        var userStatusInfoDTOs = userStatuses.Select(us => UserStatusMapper.UserStatusToUserStatusInfoDTO(us));

        return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(userStatusInfoDTOs);
    }

    public async Task<ResponseMessage<UserStatusInfoDTO>> GetUserStatusByIdAsync(Guid userStatusId)
    {
        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userStatusId);
        if (userStatus is null)
        {
            return new ResponseMessage<UserStatusInfoDTO>("User Status not Found!", 404);
        }

        var userStatusInfoDTO = UserStatusMapper.UserStatusToUserStatusInfoDTO(userStatus);

        return new ResponseMessage<UserStatusInfoDTO>(userStatusInfoDTO);
    }

    public async Task<ResponseMessage<UserStatusInfoDTO>> UpdateUserStatusAsync(Guid userStatusId, UserStatusForUpdateDTO userStatusForUpdateDTO)
    {
        var validationResult = await _userStatusForUpdateValidator.ValidateAsync(userStatusForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userStatusId);
        if (userStatus is null)
        {
            return new ResponseMessage<UserStatusInfoDTO>("User Status not Found!", 404);
        }

        UserStatusMapper.UpdateUserStatusFromUserStatusForUpdateDTO(userStatusForUpdateDTO, userStatus);
        await _repositoryManager.CommitAsync();
        var userStatusInfoDTO = UserStatusMapper.UserStatusToUserStatusInfoDTO(userStatus);

        return new ResponseMessage<UserStatusInfoDTO>(userStatusInfoDTO);
    }
}
