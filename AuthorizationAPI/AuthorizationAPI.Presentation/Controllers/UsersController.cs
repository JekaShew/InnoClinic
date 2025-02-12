using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response.FailMesssages;
using CommonLibrary.Response.SuccessMessages;
using Microsoft.AspNetCore.JsonPatch;
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
    [ProducesResponseType(typeof(SuccessMessage<UserDetailedDTO>), 200)]
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
    /// Gets the list of all Users
    /// </summary>
    /// <returns>The Users list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<UserInfoDTO>>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> GetAllUsersInfo()
    {
        var result = await _userService.GetAllUsersInfo();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<IEnumerable<UserInfoDTO>>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Updates selected User 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/{userId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> UpdateUserInfo(Guid userId, [FromBody] UserForUpdateDTO userForUpdateDTO)
    {
        var result = await _userService.UpdateUserInfo(userId, userForUpdateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }
    /// <summary>
    /// Deletes User By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/deletecurrentaccount")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> DeleteCurrentAccount()
    {
        var result = await _userService.DeleteCurrentAccount();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    /// <summary>
    /// Deletes User By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> DeleteUserById(Guid userId)
    {
        var result = await _userService.DeleteUserById(userId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 204);
    }

    /// <summary>
    /// Updates User's password by verifying Old User's Password
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/changepasswordbyoldpassword")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> ChangePasswordByOldPassword([FromBody] OldNewPasswordPairDTO oldNewPasswordPairDTO)
    {
        var result = await _userService.ChangePasswordByOldPassword(oldNewPasswordPairDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    /// <summary>
    /// Updates User's password by verifying User's Email and Secret Phrase
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("/changeforgottenpasswordbysecretphrase")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> ChangeForgottenPasswordBySecretPhrase([FromBody] EmailSecretPhraseNewPasswordDTO emailSecretPhraseNewPasswordDTO)
    {
        var result = await _userService.ChangeForgottenPasswordBySecretPhrase(emailSecretPhraseNewPasswordDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    ////Implement
    //[HttpPut("/changeforgottenpasswordbyemail")]
    //[ProducesResponseType(typeof(SuccessMessage), 200)]
    //[ProducesResponseType(typeof(FailMessage), 400)]
    //[ProducesResponseType(typeof(FailMessage), 403)]
    //[ProducesResponseType(typeof(FailMessage), 404)]
    //[ProducesResponseType(typeof(FailMessage), 408)]
    //[ProducesResponseType(typeof(FailMessage), 422)]
    //[ProducesResponseType(typeof(FailMessage), 500)]
    //public async Task<IActionResult> ChangeForgottenPasswordByEmail([FromBody] string email)
    //{
    //    return NotFound("Service  unavailable.");
    //}

    /// <summary>
    /// Changes User's status
    /// </summary>
    /// <returns>Message</returns>
    [HttpPatch("/{userId:guid}/changeuserstatusofuser")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeUserStatusOfUser(Guid userId, [FromBody] JsonPatchDocument<UserInfoDTO> patchDocForUserInfoDTO)
    {
        var result = await _userService.ChangeUserStatusOfUser(userId, patchDocForUserInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    /// <summary>
    /// Changes User's role
    /// </summary>
    /// <returns>Message</returns>
    [HttpPatch("/{userId:guid}/changeroleofuser")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeRoleOfUser(Guid userId, [FromBody] JsonPatchDocument<UserInfoDTO> patchDocForUserInfoDTO)
    {
        var result = await _userService.ChangeRoleOfUser(userId, patchDocForUserInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }
}
