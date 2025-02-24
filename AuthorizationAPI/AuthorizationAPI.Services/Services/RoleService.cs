using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using CommonLibrary.CommonService;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services;

public class RoleService : IRoleService
{
    private readonly IValidator<RoleForCreateDTO> _roleForCreateValidator;
    private readonly IValidator<RoleForUpdateDTO> _roleForUpdateValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    public RoleService(
            IRepositoryManager repositoryManager,
            ICommonService commonService,
            IValidator<RoleForCreateDTO> roleForCreateValidator,
            IValidator<RoleForUpdateDTO> roleForUpdateValidator
            )
    {
        _repositoryManager = repositoryManager;
        _commonService = commonService;
        _roleForCreateValidator = roleForCreateValidator;
        _roleForUpdateValidator = roleForUpdateValidator;
        
    }
    public async Task<ResponseMessage> CreateRoleAsync(RoleForCreateDTO roleForCreateDTO)
    {
        var validationResult = await _roleForCreateValidator.ValidateAsync(roleForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var role = RoleMapper.RoleForCreateDTOToRole(roleForCreateDTO);
        await _repositoryManager.Role.CreateRoleAsync(role);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);
    }

    public async Task<ResponseMessage> DeleteRoleByIdAsync(Guid roleId)
    {
        var role = await _repositoryManager.Role.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }
           
        _repositoryManager.Role.DeleteRole(role);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<RoleInfoDTO>>> GetAllRolesAsync()
    {
        var roles = await _repositoryManager.Role.GetAllRolesAsync();
        if (!roles.Any())
        {
            return new ResponseMessage<IEnumerable<RoleInfoDTO>>(MessageConstants.NotFoundMessage, false);
        }

        var roleInfoDTOs = roles.Select(r => RoleMapper.RoleToRoleInfoDTO(r));

        return new ResponseMessage<IEnumerable<RoleInfoDTO>>(MessageConstants.SuccessMessage, true, roleInfoDTOs);
    }

    public async Task<ResponseMessage<RoleInfoDTO>> GetRoleByIdAsync(Guid roleId)
    {
        var role = await _repositoryManager.Role.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            return new ResponseMessage<RoleInfoDTO>(MessageConstants.NotFoundMessage, false);
        }
            
        var roleInfoDTO = RoleMapper.RoleToRoleInfoDTO(role);

        return new ResponseMessage<RoleInfoDTO>(MessageConstants.SuccessMessage, true, roleInfoDTO);
    }

    public async Task<ResponseMessage> UpdateRoleAsync(Guid roleId, RoleForUpdateDTO roleForUpdateDTO)
    {
        var validationResult = await _roleForUpdateValidator.ValidateAsync(roleForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var role = await _repositoryManager.Role.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }
  
        RoleMapper.UpdateRoleFromRoleForUpdateDTO(roleForUpdateDTO, role);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }
}
