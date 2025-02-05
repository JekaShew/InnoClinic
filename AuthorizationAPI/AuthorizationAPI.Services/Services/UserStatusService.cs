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
        public async Task<ResponseMessage> CreateUserStatusAsync(UserStatusForCreateDTO userStatusForCreateDTO)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            var userStatus = UserStatusMapper.UserStatusForCreateDTOToUserStatus(userStatusForCreateDTO);
            _repositoryManager.UserStatus.CreateUserStatus(userStatus);
            await _repositoryManager.SaveChangesAsync();
            
            return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);
        }

        public async Task<ResponseMessage> DeleteUserStatusByIdAsync(Guid userStatusId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Id.Equals(userStatusId), false))
                    .FirstOrDefault();
            if (userStatus is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            _repositoryManager.UserStatus.DeleteUserStatus(userStatus);
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
        }

        public async Task<ResponseMessage<IEnumerable<UserStatusInfoDTO>>> GetAllUserStatusesAsync()
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(MessageConstants.ForbiddenMessage, false);

            var userStatuses = await _repositoryManager.UserStatus.GetAllUserStatusesAsync(false);
            if (!userStatuses.Any())
                return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(MessageConstants.NotFoundMessage, false);
            var userStatusInfoDTOs = userStatuses.Select(us => UserStatusMapper.UserStatusToUserStatusInfoDTO(us));

            return new ResponseMessage<IEnumerable<UserStatusInfoDTO>>(MessageConstants.SuccessMessage, true,  userStatusInfoDTOs);
        }

        public async Task<ResponseMessage<UserStatusInfoDTO>> GetUserStatusByIdAsync(Guid userStatusId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage<UserStatusInfoDTO>(MessageConstants.ForbiddenMessage, false);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(us => us.Id.Equals(userStatusId), false))
                    .FirstOrDefault();
            if (userStatus is null)
                return new ResponseMessage<UserStatusInfoDTO>(MessageConstants.NotFoundMessage, false);
            var userStatusInfoDTO = UserStatusMapper.UserStatusToUserStatusInfoDTO(userStatus);

            return new ResponseMessage<UserStatusInfoDTO>(MessageConstants.SuccessMessage,true, userStatusInfoDTO);
        }

        public async Task<ResponseMessage> UpdateUserStatusAsync(Guid userStatusId, UserStatusForUpdateDTO userStatusForUpdateDTO)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Id.Equals(userStatusId), true))
                    .FirstOrDefault();
            if (userStatus is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            userStatus = UserStatusMapper.UserStatusForUpdateDTOToUserStatus(userStatusForUpdateDTO);
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
        }
    }
}
