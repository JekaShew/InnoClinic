using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _userService;
        public RoleService(IRepositoryManager repositoryManager, IUserService userService)
        {
            _repositoryManager = repositoryManager; 
            _userService = userService;
        }
        public async Task<CommonResponse> CreateRoleAsync(RoleForCreateDTO roleForCreateDTO)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            var role = RoleMapper.RoleForCreateDTOToRole(roleForCreateDTO);
            _repositoryManager.Role.CreateRole(role);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessCreateMessage);
        }

        public async Task<CommonResponse> DeleteRoleByIdAsync(Guid roleId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            var role = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(roleId), false))
                    .FirstOrDefault();
            if (role is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);

            _repositoryManager.Role.DeleteRole(role);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessDeleteMessage);
        }

        public async Task<CommonResponse<IEnumerable<RoleInfoDTO>>> GetAllRolesAsync()
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse<IEnumerable<RoleInfoDTO>>(false, MessageConstants.ForbiddenMessage);

            var roles = await _repositoryManager.Role.GetAllRolesAsync(false);
            if (!roles.Any())
                return new CommonResponse<IEnumerable<RoleInfoDTO>>(false, MessageConstants.NotFoundMessage);
            var roleInfoDTOs = roles.Select( r => RoleMapper.RoleToRoleInfoDTO(r));

            return new CommonResponse<IEnumerable<RoleInfoDTO>>(true, MessageConstants.SuccessMessage, roleInfoDTOs); 
        }

        public async Task<CommonResponse<RoleInfoDTO>> GetRoleByIdAsync(Guid roleId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse<RoleInfoDTO>(false, MessageConstants.ForbiddenMessage);

            var role = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(roleId), false))
                    .FirstOrDefault();
            if (role is null)
                return new CommonResponse<RoleInfoDTO> (false, MessageConstants.NotFoundMessage);
            var roleInfoDTO = RoleMapper.RoleToRoleInfoDTO(role);

            return new CommonResponse<RoleInfoDTO>(true, MessageConstants.SuccessMessage, roleInfoDTO);
        }

        public async Task<CommonResponse> UpdateRoleAsync(Guid roleId, RoleForUpdateDTO roleForUpdateDTO)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            var role = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(roleId), true))
                    .FirstOrDefault();
            if (role is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);

            role = RoleMapper.RoleForUpdateDTOToRole(roleForUpdateDTO);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessUpdateMessage);
        }
    }
}
