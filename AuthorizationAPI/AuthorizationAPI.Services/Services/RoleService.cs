using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services;

public class RoleService : IRoleService
{
    private readonly IValidator<RoleForCreateDTO> _roleForCreateDTOValidator;
    private readonly IValidator<RoleForUpdateDTO> _roleForUpdateDTOValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly IUserService _userService;
    public RoleService(
            IRepositoryManager repositoryManager, 
            IUserService userService,
            IValidator<RoleForCreateDTO> roleForCreateDTOValidator,
            IValidator<RoleForUpdateDTO> roleForUpdateDTOValidator)
    {
        _repositoryManager = repositoryManager;
        _userService = userService;
        _roleForCreateDTOValidator = roleForCreateDTOValidator;
        _roleForUpdateDTOValidator = roleForUpdateDTOValidator;
    }
    public async Task<ResponseMessage> CreateRoleAsync(RoleForCreateDTO roleForCreateDTO)
    {
        var validationResult = await _roleForCreateDTOValidator.ValidateAsync(roleForCreateDTO);
        if (!validationResult.IsValid)
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());

        var userAdmin = await _userService.IsCurrentUserAdministrator();
        if (userAdmin is null)
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

        var role = RoleMapper.RoleForCreateDTOToRole(roleForCreateDTO);
        _repositoryManager.Role.CreateRole(role);
        await _repositoryManager.SaveChangesAsync();

        return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);
    }

    public async Task<ResponseMessage> DeleteRoleByIdAsync(Guid roleId)
    {
        var userAdmin = await _userService.IsCurrentUserAdministrator();
        if (userAdmin is null)
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

        var role = (await _repositoryManager.Role
                .GetRolesWithExpressionAsync(r => r.Id.Equals(roleId), false))
                .FirstOrDefault();
        if (role is null)
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);

        _repositoryManager.Role.DeleteRole(role);
        await _repositoryManager.SaveChangesAsync();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<RoleInfoDTO>>> GetAllRolesAsync()
    {
        var userAdmin = await _userService.IsCurrentUserAdministrator();
        if (userAdmin is null)
            return new ResponseMessage<IEnumerable<RoleInfoDTO>>(MessageConstants.ForbiddenMessage, false);

        var roles = await _repositoryManager.Role.GetAllRolesAsync(false);
        if (!roles.Any())
            return new ResponseMessage<IEnumerable<RoleInfoDTO>>(MessageConstants.NotFoundMessage, false);

        var roleInfoDTOs = roles.Select(r => RoleMapper.RoleToRoleInfoDTO(r));

        return new ResponseMessage<IEnumerable<RoleInfoDTO>>(MessageConstants.SuccessMessage, true, roleInfoDTOs);
    }

    public async Task<ResponseMessage<RoleInfoDTO>> GetRoleByIdAsync(Guid roleId)
    {
        var userAdmin = await _userService.IsCurrentUserAdministrator();
        if (userAdmin is null)
            return new ResponseMessage<RoleInfoDTO>(MessageConstants.ForbiddenMessage, false);

        var role = (await _repositoryManager.Role
                .GetRolesWithExpressionAsync(r => r.Id.Equals(roleId), false))
                .FirstOrDefault();
        if (role is null)
            return new ResponseMessage<RoleInfoDTO>(MessageConstants.NotFoundMessage, false);

        var roleInfoDTO = RoleMapper.RoleToRoleInfoDTO(role);

        return new ResponseMessage<RoleInfoDTO>(MessageConstants.SuccessMessage, true, roleInfoDTO);
    }

    public async Task<ResponseMessage> UpdateRoleAsync(Guid roleId, RoleForUpdateDTO roleForUpdateDTO)
    {
        var validationResult = await _roleForUpdateDTOValidator.ValidateAsync(roleForUpdateDTO);
        if (!validationResult.IsValid)
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());

        var userAdmin = await _userService.IsCurrentUserAdministrator();
        if (userAdmin is null)
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

        var role = (await _repositoryManager.Role
                .GetRolesWithExpressionAsync(r => r.Id.Equals(roleId), true))
                .FirstOrDefault();
        if (role is null)
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);

        role = RoleMapper.RoleForUpdateDTOToRole(roleForUpdateDTO);
        await _repositoryManager.SaveChangesAsync();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }
}
