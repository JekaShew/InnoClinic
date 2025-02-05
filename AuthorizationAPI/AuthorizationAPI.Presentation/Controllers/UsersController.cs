using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseManualController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfoById(Guid userId)
        {
            var result = await _userService.GetUserDetailedInfo(userId);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersInfo()
        {
            var result = await _userService.GetAllUsersInfo();
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        [HttpPut("/{userId}")]
        public async Task<IActionResult> UpdateUserInfo(Guid userId, [FromBody] UserForUpdateDTO userForUpdateDTO)
        {
            var result = await _userService.UpdateUserInfo(userId, userForUpdateDTO);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        //[HttpPatch("/deletecurrentaccount")]
        //public async Task<IActionResult> DeleteCurrentAccount([FromBody] UserIdRoleIdPairDTO userIdRoleIdPirDTO)
        //{
        //    var result = await _userServices.ChangeRoleOfUser(userIdRoleIdPirDTO);
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserById(Guid userId)
        {
            var result = await _userService.DeleteUserById(userId);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

       

        [HttpPut("/changepasswordbyoldpassword")]
        public async Task<IActionResult> ChangePasswordByOldPassword([FromBody] string oldPassword, string newPassword)
        {
            var result = await _userService.ChangePasswordByOldPassword(oldPassword, newPassword);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        [HttpPut("/changeforgottenpasswordbysecretphrase")]
        public async Task<IActionResult> ChangeForgottenPasswordBySecretPhrase([FromBody] EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var result = await _userService.ChangeForgottenPasswordBySecretPhrase(emailSecretPhrasePairDTO, newPassword);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
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
    }
}
