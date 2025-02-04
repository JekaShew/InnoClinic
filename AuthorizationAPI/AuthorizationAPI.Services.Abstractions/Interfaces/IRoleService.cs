using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IRoleService
    {
        public Task<CommonResponse> CreateRoleAsync(RoleForCreateDTO roleForCreateDTO);
        public Task<CommonResponse> UpdateRoleAsync(Guid roleId, RoleForUpdateDTO roleForUpdateDTO);
        public Task<CommonResponse> DeleteRoleByIdAsync(Guid roleId);
        public Task<CommonResponse<IEnumerable<RoleInfoDTO>>> GetAllRolesAsync();
        public Task<CommonResponse<RoleInfoDTO>> GetRoleByIdAsync(Guid roleId);

    }
}
