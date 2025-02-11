using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using CommonLibrary.Response.FailMesssages;
using CommonLibrary.Response.SuccessMessages;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserStatusesController : ResponseMessageHandler
{
    private readonly IUserStatusService _userStatusService;
    public UserStatusesController(IUserStatusService userStatusService)
    {
        _userStatusService = userStatusService;
    }

    /// <summary>
    /// Gets selected User Status
    /// </summary>
    /// <returns>Single User Status</returns>
    [HttpGet("{userStatusId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage<UserStatusInfoDTO>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetUserStatusById(Guid userStatusId)
    {
        var result = await _userStatusService.GetUserStatusByIdAsync(userStatusId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Gets the list of all User Statuses
    /// </summary>
    /// <returns>The User Statuses list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<UserStatusInfoDTO>>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllUserStatuses()
    {
        var result = await _userStatusService.GetAllUserStatusesAsync();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Creates new User Status
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
    public async Task<IActionResult> AddUserStatus([FromBody] UserStatusForCreateDTO userStatusForCreateDTO)
    {
        var result = await _userStatusService.CreateUserStatusAsync(userStatusForCreateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessOnCreateMessage(result.Message.Value));
    }

    /// <summary>
    /// Updates selected User Status 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/{userStatusId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateUserStatus(Guid userStatusId, [FromBody] UserStatusForUpdateDTO userStatusForUpdateDTO)
    {
        var result = await _userStatusService.UpdateUserStatusAsync(userStatusId, userStatusForUpdateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage(result.Message.Value));
    }

    /// <summary>
    /// Deletes User Status By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{userStatusId:guid}")]
    [ProducesResponseType(typeof(SuccessOnDeleteMessage), 204)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteUserStatusById(Guid userStatusId)
    {
        var result = await _userStatusService.DeleteUserStatusByIdAsync(userStatusId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessOnDeleteMessage(result.Message.Value));
    }
}
