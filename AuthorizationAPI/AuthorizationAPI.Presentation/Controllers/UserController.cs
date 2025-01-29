using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserServices _userServices;
        public UserController(IMediator mediator, IUserServices userServices)
        {
            _mediator = mediator;
            _userServices = userServices;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> TakeUserById(Guid userId)
        {
            var result = await _mediator.Send(new TakeUserDTOByIdQuery() { Id = userId});
            if (result == null)
                return NotFound("User Not found!");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> TakeAllUsers()
        {
            var result = await _mediator.Send(new TakeUserDTOListQuery() { });
            if (!result.Any())
                return NotFound("No users Found!");
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            var result = await _mediator.Send(new UpdateUserCommand() { UserDTO = userDTO });
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserById(Guid userId)
        {
            var result = await _mediator.Send(new DeleteUserByIdCommand() { Id = userId });
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        public async Task<IActionResult> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        {
            var result = await _userServices.ChangeUserStatusOfUser(userIdUserStatusIdPairDTO);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        public async Task<IActionResult> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPirDTO)
        {
            var result = await _userServices.ChangeRoleOfUser(userIdRoleIdPirDTO);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        public async Task<IActionResult> ChangePasswordByOldPassword(string oldPassword, string newPassword)
        {
            var result = await _userServices.ChangePasswordByOldPassword(oldPassword, newPassword);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        public async Task<IActionResult> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var result = await _userServices.ChangeForgottenPasswordBySecretPhrase(emailSecretPhrasePairDTO, newPassword);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        //Implement
        public async Task<IActionResult> ChangeForgottenPasswordByEmail(string email)
        {
            return NotFound("Service  unavailable.");
        }
    }
}
