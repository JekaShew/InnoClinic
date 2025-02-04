using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IUserStatusService
    {
        public Task<CommonResponse> CreateUserStatusAsync(UserStatusForCreateDTO userStatusForCreateDTO);
        public Task<CommonResponse> UpdateUserStatusAsync(Guid userStatusId, UserStatusForUpdateDTO userStatusForUpdateDTO);
        public Task<CommonResponse> DeleteUserStatusByIdAsync(Guid userStatusId);
        public Task<CommonResponse<IEnumerable<UserStatusInfoDTO>>> GetAllUserStatusesAsync();
        public Task<CommonResponse<UserStatusInfoDTO>> GetUserStatusByIdAsync(Guid userStatusId);
    }
}
