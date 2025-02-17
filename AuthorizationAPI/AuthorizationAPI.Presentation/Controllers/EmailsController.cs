using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailsController : ResponseMessageHandler
{
    // foreach check Authorize
    // foreach check Roles
    // where email metadata should be created??
    // responses?

    // for multy message noreply ONLY
    // send subject string;  send body string 

    // implement send single from doctor custom message with subject ONLY DOCTOR
    // implement send single from administrator custom message with subject ONLY ADMINISTRATOR
    // implement send single from noreply custom message with subject ONLY ADMINISTRATOR
    // implement send multy custom message from NoReply with subject ONLY ADMINISTRATOR
    private readonly IEmailService _emailService;
    public EmailsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Gets selected User 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpPost("{userId:guid}/sendfromdoctoremail")]
    [ProducesResponseType(typeof(SuccessMessage<UserDetailedDTO>), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Doctor")]
    public async Task<IActionResult> SendFromDoctorEmail(Guid userId,[FromBody] IEnumerable<UserEmailDTO> userEmailDTOs)
    {
        var result = await _emailService.SendUserEmail(userEmailDTOs,userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }

    /// <summary>
    /// Gets selected User 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpPost("{userId:guid}/sendfromadministratoremail")]
    [ProducesResponseType(typeof(SuccessMessage), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> GetUserInfoById(Guid userId)
    {
        var result = await _userService.GetUserDetailedInfo(userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<UserDetailedDTO>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Gets selected User 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> GetUserInfoById(Guid userId)
    {
        var result = await _userService.GetUserDetailedInfo(userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<UserDetailedDTO>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Gets selected User 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> GetUserInfoById(Guid userId)
    {
        var result = await _userService.GetUserDetailedInfo(userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<UserDetailedDTO>(result.Message.Value, result.Value);
    }
}
