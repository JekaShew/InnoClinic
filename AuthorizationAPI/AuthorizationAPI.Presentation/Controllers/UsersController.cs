using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response.FailMesssages;
using CommonLibrary.Response.SuccessMessages;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ResponseMessageHandler
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets selected User 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage<UserInfoDTO>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> GetUserInfoById(Guid userId)
    {
        var result = await _userService.GetUserDetailedInfo(userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Gets the list of all Users
    /// </summary>
    /// <returns>The Users list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<UserInfoDTO>>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize(Roles ="Administrator")]
    [ProducesResponseType(typeof(ServerErrorMessage), 500)]
    public async Task<IActionResult> GetAllUsersInfo()
    {
        var result = await _userService.GetAllUsersInfo();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Updates selected User 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/{userId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize]
    public async Task<IActionResult> UpdateUserInfo(Guid userId, [FromBody] UserForUpdateDTO userForUpdateDTO)
    {
        var result = await _userService.UpdateUserInfo(userId, userForUpdateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage(result.Message.Value));
    }

    //[HttpPatch("/deletecurrentaccount")]
    //public async Task<IActionResult> DeleteCurrentAccount([FromBody] UserIdRoleIdPairDTO userIdRoleIdPirDTO)
    //{
    //    var result = await _userServices.ChangeRoleOfUser(userIdRoleIdPirDTO);
    //    if (!result.Flag)
    //        return StatusCode(500, result.Message);
    //    return Ok(result.Message);
    //}

    /// <summary>
    /// Deletes User By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(SuccessOnDeleteMessage), 204)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> DeleteUserById(Guid userId)
    {
        var result = await _userService.DeleteUserById(userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessOnDeleteMessage(result.Message.Value));
    }

    /// <summary>
    /// Updates User's password by verifying Old User's Password
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/changepasswordbyoldpassword")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize]
    public async Task<IActionResult> ChangePasswordByOldPassword([FromBody] OldNewPasswordPairDTO oldNewPasswordPairDTO)
    {
        var result = await _userService.ChangePasswordByOldPassword(oldNewPasswordPairDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage(result.Message.Value));
    }

    /// <summary>
    /// Updates User's password by verifying User's Secret Phrase
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/changeforgottenpasswordbysecretphrase")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize]
    public async Task<IActionResult> ChangeForgottenPasswordBySecretPhrase([FromBody] EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
    {
        var result = await _userService.ChangeForgottenPasswordBySecretPhrase(emailSecretPhrasePairDTO, newPassword);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage(result.Message.Value));
    }

    ////Implement
    //[HttpPut("/changeforgottenpasswordbyemail")]
    //public async Task<IActionResult> ChangeForgottenPasswordByEmail([FromBody] string email)
    //{
    //    return NotFound("Service  unavailable.");
    //}

    //[HttpPatch("/changeuserstatusofuser")]
    //public async Task<IActionResult> ChangeUserStatusOfUser([FromBody] UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
    //{
    //    var result = await _userServices.ChangeUserStatusOfUser(userIdUserStatusIdPairDTO);
    //    if (!result.Flag)
    //        return StatusCode(500, result.Message);
    //    return Ok(result.Message);
    //}

    //[HttpPatch("/changeroleofuser")]
    //public async Task<IActionResult> ChangeRoleOfUser([FromBody] UserIdRoleIdPairDTO userIdRoleIdPirDTO)
    //{
    //    var result = await _userServices.ChangeRoleOfUser(userIdRoleIdPirDTO);
    //    if (!result.Flag)
    //        return StatusCode(500, result.Message);
    //    return Ok(result.Message);
    //}
}
