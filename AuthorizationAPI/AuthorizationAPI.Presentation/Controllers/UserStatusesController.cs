using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{userStatusId:guid}")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetUserStatusById(Guid userStatusId)
    {
        var result = await _userStatusService.GetUserStatusByIdAsync(userStatusId);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(new { Value = result.Value, Message = result.Message.Value });
    }

    [HttpGet]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllUserStatuses()
    {
        var result = await _userStatusService.GetAllUserStatusesAsync();
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(new {Value = result.Value, Message = result.Message.Value});
    }

    [HttpPost]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddUserStatus([FromBody] UserStatusForCreateDTO userStatusForCreateDTO)
    {
        var result = await _userStatusService.CreateUserStatusAsync(userStatusForCreateDTO);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result.Message.Value);
    }

    [HttpPut("/{userStatusId}")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateUserStatus(Guid userStatusId, [FromBody] UserStatusForUpdateDTO userStatusForUpdateDTO)
    {
        var result = await _userStatusService.UpdateUserStatusAsync(userStatusId, userStatusForUpdateDTO);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result.Message.Value);
    }

    [HttpDelete("{userStatusId}")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteUserStatusById(Guid userStatusId)
    {
        var result = await _userStatusService.DeleteUserStatusByIdAsync(userStatusId);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result.Message.Value);
    }
}
