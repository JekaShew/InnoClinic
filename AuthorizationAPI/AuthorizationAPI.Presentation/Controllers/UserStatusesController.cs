using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using CommonLibrary.Response;
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
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetUserStatusById(Guid userStatusId)
    {
        var result = await _userStatusService.GetUserStatusByIdAsync(userStatusId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<UserStatusInfoDTO>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Gets the list of all User Statuses
    /// </summary>
    /// <returns>The User Statuses list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<UserStatusInfoDTO>>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllUserStatuses()
    {
        var result = await _userStatusService.GetAllUserStatusesAsync();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<IEnumerable<UserStatusInfoDTO>>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Creates new User Status
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessMessage), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddUserStatus([FromBody] UserStatusForCreateDTO userStatusForCreateDTO)
    {
        var result = await _userStatusService.CreateUserStatusAsync(userStatusForCreateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }

    /// <summary>
    /// Updates selected User Status 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/{userStatusId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateUserStatus(Guid userStatusId, [FromBody] UserStatusForUpdateDTO userStatusForUpdateDTO)
    {
        var result = await _userStatusService.UpdateUserStatusAsync(userStatusId, userStatusForUpdateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    /// <summary>
    /// Deletes User Status By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{userStatusId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteUserStatusById(Guid userStatusId)
    {
        var result = await _userStatusService.DeleteUserStatusByIdAsync(userStatusId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 204);
    }
}
