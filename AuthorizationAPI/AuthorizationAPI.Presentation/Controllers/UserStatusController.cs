using AuthorizationAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatusController : ControllerBase
    {
        //private readonly IUserStatusServices _userStatusServices;
        //public UserStatusController()
        //{

        //}

        //[HttpGet("{userStatusId}")]
        //public async Task<IActionResult> TakeUserStatusById(Guid userStatusId)
        //{
        //    var result = await _mediator.Send(new TakeUserStatusDTOByIdQuery() { Id = userStatusId });
        //    if (result is null)
        //        return NotFound("User Status Not found!");
        //    return Ok(result);
        //}

        //[HttpGet]
        //public async Task<IActionResult> TakeAllUserStatuses()
        //{
        //    var result = await _mediator.Send(new TakeUserStatusDTOListQuery() { });
        //    if (!result.Any())
        //        return NotFound("No User Statuses Found!");
        //    return Ok(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddUserStatus([FromBody] UserStatusDTO userStatusDTO)
        //{
        //    var result = await _mediator.Send(new AddUserStatusCommand() { UserStatusDTO = userStatusDTO });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateUserStatus([FromBody] UserStatusDTO userStatusDTO)
        //{
        //    var result = await _mediator.Send(new UpdateUserStatusCommand() { UserStatusDTO = userStatusDTO });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

        //[HttpDelete("{userStatusId}")]
        //public async Task<IActionResult> DeleteUserStatusById(Guid userStatusId)
        //{
        //    var result = await _mediator.Send(new DeleteUserStatusByIdCommand() { Id = userStatusId });
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}
    }
}
