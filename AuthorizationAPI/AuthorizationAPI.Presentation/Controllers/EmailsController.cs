using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.Constants;
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
    // responses?

    private readonly IEmailService _emailService;
    public EmailsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Creates and sends Doctor's Email to single or multy consumers 
    /// </summary>
    /// <returns>Message</returns>
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
        var result = await _emailService.SendUserEmail(userEmailDTOs,userId, DBConstants.DoctorRoleId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }

    /// <summary>
    /// Creates and sends Administrator's Email to single or multy consumers 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("{userId:guid}/sendfromadministratoremail")]
    [ProducesResponseType(typeof(SuccessMessage), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> SendFromAdministratorEmail(Guid userId, [FromBody] IEnumerable<UserEmailDTO> userEmailDTOs)
    {
        var result = await _emailService.SendUserEmail(userEmailDTOs, userId, DBConstants.AdministratorRoleId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }

    /// <summary>
    /// Creates and sends NoReply Email to single or multy consumers 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("{userId:guid}/sendfromnoreplyemail")]
    [ProducesResponseType(typeof(SuccessMessage), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> SendFromNoReplyEmail(Guid userId, [FromBody] IEnumerable<UserEmailDTO> userEmailDTOs)
    {
        var result = await _emailService.SendUserEmail(userEmailDTOs, userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }
}
