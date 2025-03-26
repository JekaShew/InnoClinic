using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IRoleService
{
    public Task<ResponseMessage<Guid>> CreateRoleAsync(RoleForCreateDTO roleForCreateDTO);
    public Task<ResponseMessage<RoleInfoDTO>> UpdateRoleAsync(Guid roleId, RoleForUpdateDTO roleForUpdateDTO);
    public Task<ResponseMessage> DeleteRoleByIdAsync(Guid roleId);
    public Task<ResponseMessage<IEnumerable<RoleInfoDTO>>> GetAllRolesAsync();
    public Task<ResponseMessage<RoleInfoDTO>> GetRoleByIdAsync(Guid roleId);
}
