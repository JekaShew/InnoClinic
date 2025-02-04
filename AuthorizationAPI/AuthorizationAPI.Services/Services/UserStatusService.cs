using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services
{
    public class UserStatusService : IUserStatusService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _userService;
        public UserStatusService(IRepositoryManager repositoryManager, IUserService userService)
        {
            _repositoryManager = repositoryManager;
            _userService = userService;
        }
        public async Task<CommonResponse> CreateUserStatusAsync(UserStatusForCreateDTO userStatusForCreateDTO)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(ResponseTypes.Forbidden, false, MessageConstants.ForbiddenMessage);

            var userStatus = UserStatusMapper.UserStatusForCreateDTOToUserStatus(userStatusForCreateDTO);
            _repositoryManager.UserStatus.CreateUserStatus(userStatus);
            await _repositoryManager.SaveChangesAsync();
            
            return new CommonResponse(ResponseTypes.Ok, true, MessageConstants.SuccessCreateMessage);
        }

        public async Task<CommonResponse> DeleteUserStatusByIdAsync(Guid userStatusId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(ResponseTypes.Forbidden, false, MessageConstants.ForbiddenMessage);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Id.Equals(userStatusId), false))
                    .FirstOrDefault();
            if (userStatus is null)
                return new CommonResponse(ResponseTypes.NotFound, false, MessageConstants.NotFoundMessage);

            _repositoryManager.UserStatus.DeleteUserStatus(userStatus);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(ResponseTypes.Ok, true, MessageConstants.SuccessDeleteMessage);
        }

        public async Task<CommonResponse<IEnumerable<UserStatusInfoDTO>>> GetAllUserStatusesAsync()
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse<IEnumerable<UserStatusInfoDTO>>(ResponseTypes.Forbidden, false, MessageConstants.ForbiddenMessage);

            var userStatuses = await _repositoryManager.UserStatus.GetAllUserStatusesAsync(false);
            if (!userStatuses.Any())
                return new CommonResponse<IEnumerable<UserStatusInfoDTO>>(ResponseTypes.NotFound, false, MessageConstants.NotFoundMessage);
            var userStatusInfoDTOs = userStatuses.Select(us => UserStatusMapper.UserStatusToUserStatusInfoDTO(us));

            return new CommonResponse<IEnumerable<UserStatusInfoDTO>>(ResponseTypes.Ok, true, MessageConstants.SuccessMessage, userStatusInfoDTOs);
        }

        public async Task<CommonResponse<UserStatusInfoDTO>> GetUserStatusByIdAsync(Guid userStatusId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse<UserStatusInfoDTO>(ResponseTypes.Forbidden, false, MessageConstants.ForbiddenMessage);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(us => us.Id.Equals(userStatusId), false))
                    .FirstOrDefault();
            if (userStatus is null)
                return new CommonResponse<UserStatusInfoDTO>(ResponseTypes.NotFound, false, MessageConstants.NotFoundMessage);
            var userStatusInfoDTO = UserStatusMapper.UserStatusToUserStatusInfoDTO(userStatus);

            return new CommonResponse<UserStatusInfoDTO>(ResponseTypes.Ok, true, MessageConstants.SuccessMessage, userStatusInfoDTO);
        }

        public async Task<CommonResponse> UpdateUserStatusAsync(Guid userStatusId, UserStatusForUpdateDTO userStatusForUpdateDTO)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(ResponseTypes.Forbidden, false, MessageConstants.ForbiddenMessage);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Id.Equals(userStatusId), true))
                    .FirstOrDefault();
            if (userStatus is null)
                return new CommonResponse(ResponseTypes.NotFound, false, MessageConstants.NotFoundMessage);

            userStatus = UserStatusMapper.UserStatusForUpdateDTOToUserStatus(userStatusForUpdateDTO);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(ResponseTypes.Ok, true, MessageConstants.SuccessUpdateMessage);
        }
    }
}
