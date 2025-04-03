using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services;

public class RoleService : IRoleService
{
    private readonly IValidator<RoleForCreateDTO> _roleForCreateValidator;
    private readonly IValidator<RoleForUpdateDTO> _roleForUpdateValidator;
    private readonly IRepositoryManager _repositoryManager;

    public RoleService(
            IRepositoryManager repositoryManager,
            IValidator<RoleForCreateDTO> roleForCreateValidator,
            IValidator<RoleForUpdateDTO> roleForUpdateValidator
            )
    {
        _repositoryManager = repositoryManager;
        _roleForCreateValidator = roleForCreateValidator;
        _roleForUpdateValidator = roleForUpdateValidator;
        
    }
    public async Task<ResponseMessage<RoleInfoDTO>> CreateRoleAsync(RoleForCreateDTO roleForCreateDTO)
    {
        var validationResult = await _roleForCreateValidator.ValidateAsync(roleForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var role = RoleMapper.RoleForCreateDTOToRole(roleForCreateDTO);
        await _repositoryManager.Role.CreateRoleAsync(role);
        await _repositoryManager.CommitAsync();
        var roleInfoDTO = RoleMapper.RoleToRoleInfoDTO(role);

        return new ResponseMessage<RoleInfoDTO>(roleInfoDTO);
    }

    public async Task<ResponseMessage> DeleteRoleByIdAsync(Guid roleId)
    {
        var role = await _repositoryManager.Role.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            return new ResponseMessage("No Role Found!", 404);
        }
           
        _repositoryManager.Role.DeleteRole(role);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<IEnumerable<RoleInfoDTO>>> GetAllRolesAsync()
    {
        var roles = await _repositoryManager.Role.GetAllRolesAsync();
        var roleInfoDTOs = roles.Select(r => RoleMapper.RoleToRoleInfoDTO(r));

        return new ResponseMessage<IEnumerable<RoleInfoDTO>>(roleInfoDTOs);
    }

    public async Task<ResponseMessage<RoleInfoDTO>> GetRoleByIdAsync(Guid roleId)
    {
        var role = await _repositoryManager.Role.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            return new ResponseMessage<RoleInfoDTO>("No Role Found!", 404);
        }
            
        var roleInfoDTO = RoleMapper.RoleToRoleInfoDTO(role);

        return new ResponseMessage<RoleInfoDTO>(roleInfoDTO);
    }

    public async Task<ResponseMessage<RoleInfoDTO>> UpdateRoleAsync(Guid roleId, RoleForUpdateDTO roleForUpdateDTO)
    {
        var validationResult = await _roleForUpdateValidator.ValidateAsync(roleForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var role = await _repositoryManager.Role.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            return new ResponseMessage<RoleInfoDTO>("No Role Found!", 404);
        }
  
        RoleMapper.UpdateRoleFromRoleForUpdateDTO(roleForUpdateDTO, role);
        await _repositoryManager.CommitAsync();
        var roleInfoDTO = RoleMapper.RoleToRoleInfoDTO(role);

        return new ResponseMessage<RoleInfoDTO>(roleInfoDTO);
    }
}
