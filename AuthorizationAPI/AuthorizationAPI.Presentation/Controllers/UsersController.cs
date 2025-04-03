using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets selected User's information for User 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> GetUserInfoById(Guid userId)
    {
        var result = await _userService.GetUserInfoById(userId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Gets selected User's  detailed information for Administrator 
    /// </summary>
    /// <returns>Single User</returns>
    [HttpGet("{userId}/getbyadministrator")]
    [ProducesResponseType(typeof(UserDetailedDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles="Administrator)]
    public async Task<IActionResult> GetUserDetailedInfoById(Guid userId)
    {
        var result = await _userService.GetUserDetailedInfoById(userId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Users for Administrator
    /// </summary>
    /// <returns>The Users list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable <UserInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> GetAllUsersInfo()
    {
        var result = await _userService.GetAllUsersInfo();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Updates selected User by User
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{userId}")]
    [ProducesResponseType(200)]
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
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Updates selected User by Administrator
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{userId}/updatebyadministrator")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> UpdateUserInfoByAdministrator(Guid userId, [FromBody] UserForUpdateByAdministratorDTO userForUpdateByAdministratorDTO)
    {
        var result = await _userService.UpdateUserInfoByAdministrator(userId, userForUpdateByAdministratorDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Changes User's status to deleted by User
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("deletecurrentaccount")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> DeleteCurrentAccount()
    {
        var result = await _userService.DeleteCurrentAccount();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Deletes User By Id for Administrator
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{userId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles ="Administrator")]
    public async Task<IActionResult> DeleteUserById(Guid userId)
    {
        var result = await _userService.DeleteUserById(userId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return NoContent();
    }

    /// <summary>
    /// Updates User's password by verifying Old User's Password
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("changepasswordbyoldpassword")]
    [ProducesResponseType(200)]
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
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Updates User's password by verifying User's Email and Secret Phrase
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("changeforgottenpasswordbysecretphrase")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> ChangeForgottenPasswordBySecretPhrase([FromBody] EmailSecretPhraseNewPasswordDTO emailSecretPhraseNewPasswordDTO)
    {
        var result = await _userService.ChangeForgottenPasswordBySecretPhrase(emailSecretPhraseNewPasswordDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Changes User's status for Administrator
    /// </summary>
    /// <returns>Message</returns>
    [HttpPatch("{userId}/changeuserstatusofuser")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeUserStatusOfUser(Guid userId, [FromBody] JsonPatchDocument<UserForUpdateByAdministratorDTO> patchDocForUserInfoDTO)
    {
        var result = await _userService.ChangeUserStatusOfUser(userId, patchDocForUserInfoDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Changes User's role for Administrator
    /// </summary>
    /// <returns>Message</returns>
    [HttpPatch("{userId:guid}/changeroleofuser")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeRoleOfUser(Guid userId, [FromBody] JsonPatchDocument<UserForUpdateByAdministratorDTO> patchDocForUserInfoDTO)
    {
        var result = await _userService.ChangeRoleOfUser(userId, patchDocForUserInfoDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Changes User Status to Activated by confirming email
    /// </summary>
    /// <returns>Message</returns>
    [HttpGet("verifyemail")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token,[FromQuery] string email)
    {
        var result = await _userService.ActivateUser(email, token);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Changes User's Email by verifying Email and Password, than user should verify new Email
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("changeemailbypassword")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> ChangeEmailByPassword([FromBody] EmailPasswordPairDTO emailPasswordPairDTO)
    {
        var result = await _userService.ChangeEmailByPassword(emailPasswordPairDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }

    /// <summary>
    /// Makes verification request to User's Email to approve that it is user's desire to change password
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("changeforgottenpasswordbyemailrequest")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> ChangeForgottenPasswordByEmailRequest([FromBody] StringValue email)
    {   
        // Sends message to Email with Link -> clicks the link with token and email query parameters -> return Page where you can insert new Password
        var result = await _userService.ChangeForgottenPasswordByEmailRequest(email.Value);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    } 

    /// <summary>
    /// Verifies and Changes User's Password
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("changeforgottenpasswordbyemail")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> ChangeForgottenPasswordByEmail([FromQuery] string token, [FromQuery] string email,[FromBody] StringValue newPassword)
    {
        // page with query Parameters and body with new Password
        var result = await _userService.ChangeForgottenPasswordByEmail(token, email, newPassword.Value);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }
}
