using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using CommonLibrary.Response.FailMesssages;
using CommonLibrary.Response.SuccessMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ResponseMessageHandler
{
    private readonly IRoleService _roleService;
    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Gets selected Role
    /// </summary>
    /// <returns>Single Role</returns>
    [HttpGet("{roleId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage<RoleInfoDTO>),200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage),404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage),422)]
    [ProducesResponseType(typeof(ServerErrorMessage),500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetRoleById(Guid roleId)
    {
        var result = await _roleService.GetRoleByIdAsync(roleId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Gets the list of all Roles
    /// </summary>
    /// <returns>The Roles list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<RoleInfoDTO>>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Creates new Role
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessOnCreateMessage), 201)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddRole([FromBody] RoleForCreateDTO roleForCreateDTO)
    {
        var result = await _roleService.CreateRoleAsync(roleForCreateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(result.Message.Value);
    }

    /// <summary>
    /// Updates selected Role 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{roleId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] RoleForUpdateDTO roleForUpdateDTO)
    {
        var result = await _roleService.UpdateRoleAsync(roleId, roleForUpdateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(result.Message.Value);
    }

    /// <summary>
    /// Deletes Role By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{roleId:guid}")]
    [ProducesResponseType(typeof(SuccessOnDeleteMessage), 204)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRoleById(Guid roleId)
    {
        var result = await _roleService.DeleteRoleByIdAsync(roleId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(result.Message.Value);
    }
}
