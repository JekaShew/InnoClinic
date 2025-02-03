using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly IUserService _userServices;
        //public UserController( IUserService userServices)
        //{
        //    _userServices = userServices;
        //}

        //[HttpGet("{userId}")]
        //public async Task<IActionResult> TakeUserById(Guid userId)
        //{
        //    var result = await _mediator.Send(new TakeUserDTOByIdQuery() { Id = userId });
        //    if (result == null)
        //        return NotFound("User Not found!");
        //    return Ok(result);
        //}

        //[HttpGet]
        //public async Task<IActionResult> TakeAllUsers()
        //{
        //    var result = await _mediator.Send(new TakeUserDTOListQuery() { });
        //    if (!result.Any())
        //        return NotFound("No users Found!");
        //    return Ok(result);
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateUser([FromBody] UserDetailedDTO userDTO)
        //{
        //    var result = await _mediator.Send(new UpdateUserCommand() { UserDTO = userDTO });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpDelete("{userId}")]
        //public async Task<IActionResult> DeleteUserById(Guid userId)
        //{
        //    var result = await _mediator.Send(new DeleteUserByIdCommand() { Id = userId });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpPatch("/changeuserstatusofuser")]
        //public async Task<IActionResult> ChangeUserStatusOfUser([FromBody] UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        //{
        //    var result = await _userServices.ChangeUserStatusOfUser(userIdUserStatusIdPairDTO);
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpPatch("/changeroleofuser")]
        //public async Task<IActionResult> ChangeRoleOfUser([FromBody] UserIdRoleIdPairDTO userIdRoleIdPirDTO)
        //{
        //    var result = await _userServices.ChangeRoleOfUser(userIdRoleIdPirDTO);
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpPatch("/changepasswordbyoldpassword")]
        //public async Task<IActionResult> ChangePasswordByOldPassword([FromBody] string oldPassword, string newPassword)
        //{
        //    var result = await _userServices.ChangePasswordByOldPassword(oldPassword, newPassword);
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpPatch("/changeforgottenpasswordbysecretphrase")]
        //public async Task<IActionResult> ChangeForgottenPasswordBySecretPhrase([FromBody] EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        //{
        //    var result = await _userServices.ChangeForgottenPasswordBySecretPhrase(emailSecretPhrasePairDTO, newPassword);
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        ////Implement
        //[HttpPatch("/changeforgottenpasswordbyemail")]
        //public async Task<IActionResult> ChangeForgottenPasswordByEmail([FromBody] string email)
        //{
        //    return NotFound("Service  unavailable.");
        //}
    }
}
