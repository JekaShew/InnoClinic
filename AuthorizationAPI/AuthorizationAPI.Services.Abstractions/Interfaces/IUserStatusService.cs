using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IUserStatusService
    {
        public Task<ResponseMessage> CreateUserStatusAsync(UserStatusForCreateDTO userStatusForCreateDTO);
        public Task<ResponseMessage> UpdateUserStatusAsync(Guid userStatusId, UserStatusForUpdateDTO userStatusForUpdateDTO);
        public Task<ResponseMessage> DeleteUserStatusByIdAsync(Guid userStatusId);
        public Task<ResponseMessage<IEnumerable<UserStatusInfoDTO>>> GetAllUserStatusesAsync();
        public Task<ResponseMessage<UserStatusInfoDTO>> GetUserStatusByIdAsync(Guid userStatusId);
    }
}
