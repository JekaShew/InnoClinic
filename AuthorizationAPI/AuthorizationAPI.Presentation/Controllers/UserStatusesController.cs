using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatusesController : ControllerBase
    {
        private readonly IUserStatusService _userStatusService;
        public UserStatusesController(IUserStatusService userStatusService)
        {
            _userStatusService = userStatusService;
        }

        [HttpGet("{userStatusId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> TakeUserStatusById(Guid userStatusId)
        {
            var result = await _userStatusService.GetUserStatusByIdAsync(userStatusId);
            if (result.Flag == false)
                return result.ResponseType;
            return Ok(result);
        }

        //[HttpGet]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> TakeAllUserStatuses()
        //{
        //    var result = await _mediator.Send(new TakeUserStatusDTOListQuery() { });
        //    if (!result.Any())
        //        return NotFound("No User Statuses Found!");
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> AddUserStatus([FromBody] UserStatusDTO userStatusDTO)
        //{
        //    var result = await _mediator.Send(new AddUserStatusCommand() { UserStatusDTO = userStatusDTO });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpPut]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> UpdateUserStatus([FromBody] UserStatusDTO userStatusDTO)
        //{
        //    var result = await _mediator.Send(new UpdateUserStatusCommand() { UserStatusDTO = userStatusDTO });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpDelete("{userStatusId}")]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> DeleteUserStatusById(Guid userStatusId)
        //{
        //    var result = await _mediator.Send(new DeleteUserStatusByIdCommand() { Id = userStatusId });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}
    }
}
